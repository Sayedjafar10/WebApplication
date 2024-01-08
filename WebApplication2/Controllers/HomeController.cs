using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using WebApplication2.Models;

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
            //var cvs = _cvService.GetAllCVs();
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
            List<Project> projectsToShow;

            if (User.Identity.IsAuthenticated)
            {
               
                projectsToShow = _cvContext.Projekts.ToList();
            }
            else
            {
               
                projectsToShow = _cvContext.Projekts
                                            .Where(p => _cvContext.Users
                                                            .Any(u => u.Id == p.CreatorID && u.ProfileType == ProfileType.Public))
                                            .ToList();
            }

            ViewBag.Meddelande = "Totalt antal projekt i databasen: " + projectsToShow.Count.ToString();
            ViewBag.CurrentUser = _userManager.GetUserId(User);
            return View(projectsToShow);
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
        public IActionResult SkapaProjekt()
        {
            return View(new Project());


        }

        // En post metod som skapar och lagrar projektet i databasen 'CVDatabas' och i tabellen 'dbo.Projekts'
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

		[HttpPost]
		public IActionResult DeleteProjekt(int id)
		{
			var projekt = _cvContext.Projekts.Find(id);

			if (projekt != null)
			{
				_cvContext.Projekts.Remove(projekt);
				_cvContext.SaveChanges();
				ViewBag.Meddelande = "Projektet har raderats.";
			}
			else
			{
				ViewBag.Meddelande = "Projektet hittades inte.";
			}

			var cvs = _cvContext.Projekts.ToList();
			return View("VaraProjekt", cvs);
		}






		//[HttpPost]
		//public IActionResult Add(CV cv)
		//{
		//    _cvService.AddCV(cv);
		//    return RedirectToAction("Index");
		//}

		// Metoder för att redigera och ta bort CV kan läggas till här
	} 
}

