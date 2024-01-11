using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using DataLayer.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
  
        private readonly CVContext _cvContext;
        private readonly UserManager<User> _userManager;    

        public HomeController(CVContext cvContext, UserManager<User> userManager)
        {
            _cvContext = cvContext;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            try
            {
                Project senasteProjekt = _cvContext.Projekts.OrderByDescending(p => p.StartTime).FirstOrDefault();
                ViewBag.SenasteProjekt = senasteProjekt ?? new Project { Namn = "Det finns inga projekt" };
            }
            catch (Exception ex)
            {
                ViewBag.SenasteProjekt = new Project { Namn = "Det finns inga projekt" };
            }

            List<User> offentligaAnvandare = _userManager.Users
                .Where(u => u.ProfileType == ProfileType.Public)
                .Take(3)
                .ToList();

            ViewBag.OffentligaAnvandare = offentligaAnvandare;

            return View();
        }



        [Authorize]
        public IActionResult VaraCV()
        {
            var cvs = _cvContext.CVs.ToList(); 
            ViewBag.Meddelande = "Totalt antal CVn i databasen: " + _cvContext.CVs.Count().ToString();
            return View(cvs);
        }
      







		public IActionResult VaraProjekt()
		{
			try
			{
				List<Project> projectsToShow;

				// Kontrollera om användaren är inloggad
				if (User.Identity.IsAuthenticated)
				{
					
					projectsToShow = _cvContext.Projekts.ToList();
				}
				else
				{
					// Om användaren inte är inloggad, hämta endast public projekt
					projectsToShow = _cvContext.Projekts
											   .Where(p => _cvContext.Users
															   .Any(u => u.Id == p.CreatorID && u.ProfileType == ProfileType.Public))
											   .ToList();
				}

				
				ViewBag.Meddelande = "Totalt antal projekt i databasen: " + projectsToShow.Count.ToString();
				
				ViewBag.CurrentUser = _userManager.GetUserId(User);

				return View(projectsToShow);
			}
			catch (Exception ex)
			{
				
				return RedirectToAction("Error"); 
			}
		}

		public IActionResult GaMedProjekt(string userId, int projectId) 
        {
            var userParticipation = new UserParticipationProject
            {
                UserID = userId,
                ProjectID = projectId
            };
            _cvContext.UserParticipationProjects.Add(userParticipation);
            _cvContext.SaveChanges();
            return RedirectToAction("VaraProjekt"); 
        }


        // Retunerar SkapaProjekt vyn med skapandet av en ny produkt där model i vyn har objekt typen projekt
        [Authorize]
        public IActionResult SkapaProjekt()
        {
            return View(new Project());


        }

        
        [HttpPost]
        [Authorize]
        public IActionResult SkapaProjekt(Project projekt)
        {

         
            // Om valideringen är korrekt
            if (ModelState.IsValid)
            {
                // Gör detta
                projekt.CreatorID = _userManager.GetUserId(User);
                _cvContext.Projekts.Add(projekt);
                _cvContext.SaveChanges();
                return RedirectToAction("VaraProjekt");

            }
            else
            {   // annars
                return View(projekt);
            }

        }

        [HttpGet]
        public IActionResult RedigeraProjekt(int projektid)
        {
            var projekt = _cvContext.Projekts.SingleOrDefault(x => x.Id == projektid);
            return View(projekt);

        }


        [HttpPost]
        public IActionResult RedigeraProjekt(Project model)
        {
            if (ModelState.IsValid)
            {
                var existingProjekt = _cvContext.Projekts.SingleOrDefault(x => x.Id == model.Id);

                if (existingProjekt != null)
                {
                    // Uppdatera projektet med de nya värdena
                    existingProjekt.Namn = model.Namn;
                    existingProjekt.Beskrivning = model.Beskrivning;
                    existingProjekt.StartTime = model.StartTime;
                    existingProjekt.EndTime = model.EndTime;

                    // Uppdatera Databasen
                    _cvContext.Projekts.Update(existingProjekt);
                    // Spara ändringarna i databasen
                    _cvContext.SaveChanges();

                    
                    return RedirectToAction("VaraProjekt"); 
                }
            }
            return View(model);
        }
		public IActionResult ProjektDeltagare()
		{
			var projektMedDeltagare = _cvContext.Projekts
				.Select(p => new
				{
					ProjektNamn = p.Namn,
					AntalDeltagare = p.UsersParticipationsProjects.Count 
				}).ToList();

			return View(projektMedDeltagare);
		}


	}
}

