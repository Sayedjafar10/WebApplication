using System.ComponentModel.DataAnnotations;

/* Den här klassen kommer användas för att skicka meddelanden och
   skickas med till den controller action som hanterar skickade meddelanden */

namespace WebApplication2.Models
{
	public class SendMessageViewModel
	{
		public string SenderName { get; set; } // Namn på den som skickar, används för anonyma användare

		[Required]
		public string Content { get; set; } // Meddelandets innehåll / text

		public string ReceiverId { get; set; } // Den som mottar meddelandet måste skickas med på något sätt
	}
}
