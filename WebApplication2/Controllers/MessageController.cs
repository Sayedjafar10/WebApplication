using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

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
            // Hämta en lista över tillgängliga användare från databasen
            var availableUsers = _cvContext.Users.ToList();

            // Skicka med användarlistan till vyn
            return View(availableUsers);
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



        //Finns massa grejer ovan IDK vem som skrivit det men jag bortser från det och lägger in min kod här under /Denis
        //Jag har även skapat en MessageViewModel 


        // Visa meddelande "form" för att skapa ett meddelande (skillnad på skapa och skicka)
        public IActionResult CreateMessage(string receiverId)
        {
            var model = new SendMessageViewModel { ReceiverId = receiverId };
            return View(model);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
  
        // Kalla på denna action för att skicka meddelande
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var meddelande = new Meddelande
                {
                    ReceiverId = model.ReceiverId,
                    Content = model.Content,
                    Timestamp = DateTime.Now,
                    IsRead = false
                };

                if (User.Identity.IsAuthenticated)
                {
                    // För inloggade användare:
                    var user = await _userManager.GetUserAsync(User);
                    meddelande.SenderId = user.Id; // Hämta Id från inloggad User (VI HAR INTE ID PÅ USER)
                    meddelande.SenderName = user.Name; // Alternativt kan vi sätta SenderName från Users namn
                }
                else
                {
                    // För anonyma användare som inte är inloggade
                    meddelande.SenderName = model.SenderName; // Använd namnet som användaren matar in
                }


                // Logik för att spara meddelande till databasen
                _cvContext.Meddelanden.Add(meddelande);
                await _cvContext.SaveChangesAsync();


                return RedirectToAction("Success"); // Omdirigera till en annan sida
            }

            // Hantera error eller återgå till form för att skicka meddelande
            return View(model);
        }*/



        // Action för att visa mottagna meddelanden
        /*public async Task<IActionResult> ReceivedMessages()
        {
            var userId = _userManager.GetUserId(User);
            var messages = await _meddelandeService.GetMessagesByUserIdAsync(userId);

            var messageViewModels = messages.Select(m => new MessageViewModel
            {
                MessageId = m.MessageId,
                Content = m.Content,
                Timestamp = m.Timestamp,
                IsRead = m.IsRead,
                SenderId = m.SenderId,
                SenderName = m.SenderName
            }).ToList();

            return View(messageViewModels);
        }


        // Action för att markera meddelande som läst

        [HttpGet]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            await _meddelandeService.MarkMessageAsReadAsync(messageId);

            // Omdirigera Redirect back to the list of received messages, or to a confirmation page
            return RedirectToAction("ReceivedMessages");
        }*/



    }
}
