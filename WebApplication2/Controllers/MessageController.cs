using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using DataLayer.Models;

namespace WebApplication2.Controllers
{
    public class MessageController : Controller
    {
        private readonly CVContext _cvContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly MessageService _messageService;

        public MessageController(CVContext cvContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _cvContext = cvContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Conversation(string id) 
        {
            ViewBag.Messages = _cvContext.Meddelanden.ToList();
            ViewBag.CVUser = id;
            ViewBag.CurrentUser = _userManager.GetUserId(User);
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Meddelande meddelande) 
        {
            meddelande.SenderId = _userManager.GetUserId(User);
            meddelande.Timestamp = DateTime.Now;
            _cvContext.Meddelanden.Add(meddelande);
            _cvContext.SaveChanges();
            return RedirectToAction("Conversation", new {id = meddelande.ReceiverId});
        }

        public IActionResult Users()
        {
            // Hämta de tillgängliga användare från databasen
            var availableUsers = _cvContext.Users.ToList();

            // Skicka med användarlistan till vyn
            return View(availableUsers);
        }



        // metod för att markera / uppdatera ett meddelandes "IsRead" status
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var message = await _cvContext.Meddelanden.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _cvContext.SaveChangesAsync();
            }
            return RedirectToAction("Conversation", new {id = message.ReceiverId});
        }


        // hämta ANTALET olästa meddelanden för den inloggade användaren

        public async Task<int> GetUnreadMessageCount()
        {
            var userId = _userManager.GetUserId(User);
            var unreadCount = await _cvContext.Meddelanden.CountAsync(m => m.ReceiverId == userId && !m.IsRead);
            return unreadCount;
        }




        // GET: MessageController   
        public ActionResult Index()
        {
            return View();
        }

        // GET: MessageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MessageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MessageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



      

        // Visa meddelande "form" för att skapa ett meddelande (skillnad på skapa och skicka)
        public IActionResult CreateMessage(string receiverId)
        {
            var model = new SendMessageViewModel { ReceiverId = receiverId };
            return View(model);
        }

       

    }
}
