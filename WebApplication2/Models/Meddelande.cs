using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Exchange.WebServices.Data;

namespace WebApplication2.Models
{
	public class Meddelande
	{
		[Key]
		public int MessageId { get; set; }  // Ändrade namn på variable så att sql ska veta att det en PK, DETTA VAR ETT AV ERRORS  // Sayed 
											//  public int MessageId { get; set; }
		public string Content { get; set; } // Meddelandets innehåll
		public DateTime Timestamp { get; set; } // Tid då meddelandet skickas
		public bool IsRead { get; set; } // Checkar om meddelandet är läst eller inte



		// När en inloggad användare skickar ett meddelande så får SenderId automatiskt ett värde från UserManager??
		// Anonyma användare får null på SenderId och måste sedan input sitt namn som SenderName som skickas med istället
		public string? SenderId { get; set; } // "Nullable" främmande nyckel från Usertabellen - (PROBLEM vi har inte ID för User)
		//public string SenderName { get; set; } // Används istället för SenderId när anonym användare skickar meddelande

		//Ska vi kanske bara använda SenderName - hämta Namn från Usertabellen och om man inte är inloggad får man skriva sitt namn

		public string ReceiverId { get; set; } // AnvändarId för den som tar emot meddelandet



		// "Navigation properties"
		[ForeignKey(nameof(SenderId))]
		public virtual User Sender { get; set; }
		[ForeignKey(nameof(ReceiverId))]
		public virtual User Receiver { get; set; }

		//public virtual string ReceiverName { get; set;}    // Detta kom upp som förslag
	}
}
