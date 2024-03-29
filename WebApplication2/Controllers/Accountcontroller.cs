﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using DataLayer.Models;
using Microsoft.IdentityModel.Tokens;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly CVContext _cvContext;
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, CVContext cvContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _cvContext = cvContext;
    }
    public IActionResult HittaCV()
    {
        // Hämta en lista över de  tillgängliga användare från databasen
        var availableUsers = _cvContext.Users.ToList();

        // Skicka med användarlistan till vyn
        return View(availableUsers);
    }
    [HttpGet]
    public IActionResult VisaCV(string userId)
    {
        // Dirigera till CVSidan med den valda användarens ID
        return RedirectToAction("UserCV", new { id = userId });
    }

    [HttpPost]
    public async Task<IActionResult> Rega(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                ProfileType = model.ProfileType, // Lägg till detta
                ProfilePictureUrl = "defaultProfilePicUrl.jpg" // Lägg till standard URL här
            };



            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var cv = new CV
                {
                    UserId = _userManager.GetUserId(User)
                };
                _cvContext.CVs.Add(cv);
                _cvContext.SaveChanges();
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }


    public IActionResult UserCV(string id)
    {
        ViewBag.CurrentUser = _userManager.GetUserId(User);
        if (id.IsNullOrEmpty())
        {
            //Används när du klickar Mitt CV
            return View(targetCV(_userManager.GetUserId(User)));
        }
        //Används när du klickar ett cv i listan över cvn
        return View(targetCV(id));
    }

    private CV targetCV(string id)
    {
        var cvs = _cvContext.CVs.ToList();
        CV myCV = new CV();
        foreach (var cv in cvs)
        {
            if (cv.UserId == id)
            {
                myCV = cv;
                break;
            }
        }
        return myCV;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [HttpGet]
    public IActionResult login()
    {
        LoginViewModel loginViewModel = new LoginViewModel();
        return View(loginViewModel);
    }
    // Användare loggar in
    [HttpPost]
    public async Task<IActionResult> login(LoginViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ogiltigt inloggningsförsök.");
            }
        }

        return View(model);
    }

    


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> UpdateUser()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new UpdateUserViewModel
        {
            Email = user.Email,
            Name = user.Name,
            ProfileType = user.ProfileType
            // Sätt andra fält här från användarens data
        };
        return View(model);
    }

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
	{
		if (ModelState.IsValid)
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user != null)
				{
					// Uppdaterar användarens information
					user.Email = model.Email;
					user.Name = model.Name;
					user.ProfileType = model.ProfileType;
					

				
					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						// Om uppdateringen lyckas, omdirigera användaren
						return RedirectToAction("Index", "Home");
					}

					
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			catch (Exception ex)
			{
				
				//  ett generellt felmeddelande
				ModelState.AddModelError("", "Ett fel inträffade .");
			}
		}
		
		return View(model);
	}




	[HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Loggar ut användaren
        await _signInManager.SignOutAsync();

        // Återvänder till startsidan
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(); 
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		try
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				// om användaren hittades inte, omdirigerar  sidan till inloggningssidan
				return RedirectToAction("Login");
			}

		
			var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				// ifall lösenordsändringen lyckas, uppdatera inloggningssessionen och omdirigera till startsidan
				await _signInManager.RefreshSignInAsync(user);
				return RedirectToAction("Index", "Home");
			}

			
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
		catch (Exception ex)
		{
			
			ModelState.AddModelError("", "Ett fel inträffade vid bearbetning av din begäran.");
		}

		
		return View(model);
	}


	public async Task<IActionResult> ShowSearchForm(SearchModel searchModel)
    {
        return View(searchModel);
    }
    public ActionResult Index(SearchModel searchModel)
    {
        var searchResults = _cvContext.Users
             .Where(e => e.Name.Contains(searchModel.SearchText)) //|| e.Description.Contains(searchModel.SearchText))
             .ToList();

        // Skicka sökresultaten till vyn

        return View("~/Views/Account/HittaCV.cshtml", searchResults);
    }


    [HttpGet]
    public IActionResult AddKompetens()
    {
        return View(new Kompetens());
    }

    [HttpPost]
    public async Task<IActionResult> AddKompetens(Kompetens model)
    {
        var userId = _userManager.GetUserId(User);
        var cv = _cvContext.CVs.FirstOrDefault(c => c.UserId == userId);
        if (cv != null)
        {
            var cvKompetens = new CVKompetens { CVId = cv.Id, Kompetens = model };
            _cvContext.CVKompetenser.Add(cvKompetens);
            await _cvContext.SaveChangesAsync();

            return RedirectToAction("UserCV");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult AddErfarenhet()
    {
        return View(new Erfarenhet());
    }

    [HttpPost]
    public async Task<IActionResult> AddErfarenhet(Erfarenhet model)
    {
        var userId = _userManager.GetUserId(User);
        var cv = _cvContext.CVs.FirstOrDefault(c => c.UserId == userId);
        if (cv != null)
        {
            var cvErfarenhet = new CVErfarenhet { CVId = cv.Id, Erfarenhet = model };
            _cvContext.CVErfarenheter.Add(cvErfarenhet);
            await _cvContext.SaveChangesAsync();

            return RedirectToAction("UserCV");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult AddUtbildning()
    {
        return View(new Utbildning());
    }

    [HttpPost]
  
	[HttpPost]
	public async Task<IActionResult> AddUtbildning(Utbildning model)
	{
		try
		{
			var userId = _userManager.GetUserId(User);
			var cv = _cvContext.CVs.FirstOrDefault(c => c.UserId == userId);
			if (cv != null)
			{
				var cvUtbildning = new CVUtbildning { CVId = cv.Id, Utbildning = model };
				_cvContext.CVUtbildningar.Add(cvUtbildning);
				await _cvContext.SaveChangesAsync();

				return RedirectToAction("UserCV");
			}

			
			ModelState.AddModelError("", "CV hittades ej .");
			return View(model);
		}
		catch (Exception ex)
		{
			
			ModelState.AddModelError("", "Ett fel inträffat.");

			return View(model);
		}
	}




	[HttpGet]
    public IActionResult Upload()
    {
        var files = _cvContext.UploadedFiles.ToList();
        return View(files); // Visa alla uppladdade filer
    }

	[HttpPost]
	public async Task<IActionResult> Upload(IFormFile file)
	{
		
		if (file != null && file.Length > 0)
		{
			// Hämta filens namn
			var fileName = Path.GetFileName(file.FileName);
			// Skapa en sökväg 
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

			// Skapa en ny filstream 
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			
			var uploadedFile = new UploadedFile { FileName = fileName };
			_cvContext.UploadedFiles.Add(uploadedFile);
		
			await _cvContext.SaveChangesAsync();

		
			return RedirectToAction("Upload");
		}

		return View();
	}






	[HttpPost]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        var fileToDelete = await _cvContext.UploadedFiles.FindAsync(fileId);
        if (fileToDelete != null)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileToDelete.FileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _cvContext.UploadedFiles.Remove(fileToDelete);
            await _cvContext.SaveChangesAsync();
        }

        return RedirectToAction("Upload");
    }









    [HttpPost]
    public async Task<IActionResult> UploadCVImage(IFormFile file, string cvId)
    {
        if (file != null && file.Length > 0)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cvuploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Antag att du har en entitet som heter CVImage eller liknande
            var cvImage = new CVImage
            {
                FileName = fileName,
                CVId = cvId
            };
            _cvContext.CVImages.Add(cvImage);
            await _cvContext.SaveChangesAsync();

            // Omdirigera användaren tillbaka till CV-sidan 
            return RedirectToAction("UserCV");
        }

        return View("Error"); 
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCVImage(int id)
    {
        var cvImage = await _cvContext.CVImages.FindAsync(id);
        if (cvImage != null)
        {
            // Ta bort filen från servern
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cvuploads", cvImage.FileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

          
            _cvContext.CVImages.Remove(cvImage);
            await _cvContext.SaveChangesAsync();

          
            return RedirectToAction("UserCV"); 
        }

        return View("Error"); 
    }


   








    public IActionResult ErfarenhetLista()
    {
        // Hämta ID för den aktuellt inloggade användaren
        var userId = _userManager.GetUserId(User);

        // Hämta användarens CV
        var userCV = _cvContext.CVs.Include(cv => cv.CVErfarenheter)
                                   .ThenInclude(ce => ce.Erfarenhet)
                                   .FirstOrDefault(cv => cv.UserId == userId);

        if (userCV != null)
        {
            // Hämta lista av erfarenheter kopplade till användarens CV
            var erfarenheter = userCV.CVErfarenheter.Select(ce => ce.Erfarenhet).ToList();

            return View(erfarenheter);
        }

       
        return View(new List<Erfarenhet>());
    }



    public IActionResult EditKompetens(int id)
    {
        var kompetens = _cvContext.Kompetenser.FirstOrDefault(k => k.Id == id);
        if (kompetens == null)
        {
            return NotFound();
        }
        return View(kompetens);
    }

    public IActionResult KompetensList()
    {
       
        var userId = _userManager.GetUserId(User);

        // Hämta användarens CV
        var userCV = _cvContext.CVs.Include(cv => cv.CVKompetenser)
                                   .ThenInclude(ck => ck.Kompetens)
                                   .FirstOrDefault(cv => cv.UserId == userId);

        if (userCV != null)
        {
            // Hämta lista av kompetenser kopplade till användarens CV
            var kompetenser = userCV.CVKompetenser.Select(ck => ck.Kompetens).ToList();

            return View(kompetenser);
        }

    
        return View(new List<Kompetens>());
    }



    public IActionResult EditErfarenhet(int id)
    {
        var erfarenhet = _cvContext.Erfarenheter.FirstOrDefault(e => e.Id == id);
        if (erfarenhet == null)
        {
            return NotFound();
        }
        return View(erfarenhet);
    }
    [HttpPost]
    public async Task<IActionResult> EditKompetens(Kompetens model)
    {
        var kompetens = await _cvContext.Kompetenser.FindAsync(model.Id);
        if (kompetens != null)
        {
            kompetens.Titel = model.Titel;
            kompetens.Beskrivning = model.Beskrivning;
            _cvContext.Update(kompetens);
            await _cvContext.SaveChangesAsync();
            return RedirectToAction("KompetensList"); // Namnet på vyn där du listar alla kompetenser
        }
        return View(model);
    }

    
   
    [HttpPost]
    public async Task<IActionResult> EditErfarenhet(Erfarenhet model)
    {
        var erfarenhet = await _cvContext.Erfarenheter.FindAsync(model.Id);
        if (erfarenhet != null)
        {
            erfarenhet.Title = model.Title;
            erfarenhet.Beskrivning = model.Beskrivning;
            _cvContext.Update(erfarenhet);
            await _cvContext.SaveChangesAsync();
            return RedirectToAction("ErfarenhetLista");
        }
        return View(model);
    }


   

    public IActionResult UtbildningLista()
    {
        
        var userId = _userManager.GetUserId(User);

        // Hämta användarens CV
        var userCV = _cvContext.CVs.Include(cv => cv.CVUtbildningar)
                                   .ThenInclude(cu => cu.Utbildning)
                                   .FirstOrDefault(cv => cv.UserId == userId);

        if (userCV != null)
        {
            // Hämta lista av utbildningar kopplade till användarens CV
            var utbildningar = userCV.CVUtbildningar.Select(cu => cu.Utbildning).ToList();

            return View(utbildningar);
        }

    
        return View(new List<Utbildning>());
    }

    public IActionResult EditUtbildning(int id)
    {
        var utbildning = _cvContext.Utbildningar.FirstOrDefault(u => u.Id == id);
        if (utbildning == null)
        {
            return NotFound();
        }
        return View(utbildning);
    }
    [HttpPost]
    public async Task<IActionResult> EditUtbildning(Utbildning model)
    {
        var utbildning = await _cvContext.Utbildningar.FindAsync(model.Id);
        if (utbildning != null)
        {
            utbildning.Namn = model.Namn;
            utbildning.Beskrivning = model.Beskrivning;
            _cvContext.Update(utbildning);
            await _cvContext.SaveChangesAsync();
            return RedirectToAction("UtbildningLista");
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

        ViewBag.ProjektMedDeltagare = projektMedDeltagare;
        return View();
    }




}