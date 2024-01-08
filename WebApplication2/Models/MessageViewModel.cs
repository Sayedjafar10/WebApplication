using System.ComponentModel.DataAnnotations;

//Denna klass ska visa received messages, blanda inte ihop med "SendMessageViewModel"

namespace WebApplication2.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string Content { get; set; } // Meddelandets text / innehåll
        public DateTime Timestamp { get; set; } // Tidpunkt då meddelandet skickades
        public bool IsRead { get; set; } // Indikerar om meddelandet har lästs eller inte
        public string SenderId { get; set; } // ID på den som skicakt (null för anonyma användare)
        public string SenderName { get; set; } // Namn på den som skickat (används för anonyma användare)

    }
}
