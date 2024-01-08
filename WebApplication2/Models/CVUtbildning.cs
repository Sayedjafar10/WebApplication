using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class CVUtbildning
    {
        // Referensnycklar för relationen till Erfarenhet och CV
        [ForeignKey(nameof(CV))]
        public string CVId { get; set; }

        [ForeignKey(nameof(Utbildning))]
        public int UtbildningId { get; set; }

        // Navigeringsegenskaper till CV och Utbildning
        public virtual CV CV { get; set; }
        public virtual Utbildning Utbildning { get; set; }
    }

}
