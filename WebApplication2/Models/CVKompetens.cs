using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class CVKompetens
    {
        // Referensnycklar för relationen till CV och Kompetens
        [ForeignKey(nameof(CV))]
        public string CVId { get; set; }

        [ForeignKey(nameof(Kompetens))]
        public int KompetensId { get; set; }

        // Navigeringsegenskaper till CV och Kompetens
        public virtual CV CV { get; set; }
        public virtual Kompetens Kompetens { get; set; }
    }

}
