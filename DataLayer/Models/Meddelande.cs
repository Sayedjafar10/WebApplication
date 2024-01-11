using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Exchange.WebServices.Data;

namespace DataLayer.Models
{
	public class Meddelande
	{
		[Key]
		public int MessageId { get; set; }  
											
		public string Content { get; set; } // Meddelandets innehåll
		public DateTime Timestamp { get; set; } // Tid då meddelandet skickas
		public bool IsRead { get; set; } // Checkar om meddelandet är läst eller inte



		public string? SenderId { get; set; } 
		public string? SenderName { get; set; } 

		

		public string ReceiverId { get; set; } // AnvändarId för den som tar emot meddelandet



		
		[ForeignKey(nameof(SenderId))]
		public virtual User Sender { get; set; }
		[ForeignKey(nameof(ReceiverId))]
		public virtual User Receiver { get; set; }

		//public virtual string ReceiverName { get; set;}    // Detta kom upp som förslag
	}
}
