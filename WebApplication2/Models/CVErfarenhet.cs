using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
	public class CVErfarenhet
	{
        // Referensnycklar för relationen till Erfarenhet och CV
        [ForeignKey(nameof(Erfarenhet))]
        public int ErfarenhetId { get; set; }

        [ForeignKey(nameof(CV))]
        public string CVId { get; set; }

        // Navigeringsegenskaper till Erfarenhet och CV
        public virtual Erfarenhet Erfarenhet { get; set; }
        public virtual CV CV { get; set; }
    }
}
