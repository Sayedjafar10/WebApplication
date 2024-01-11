using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class CVKompetens
    {
        
        [ForeignKey(nameof(CV))]
        public string CVId { get; set; }

        [ForeignKey(nameof(Kompetens))]
        public int KompetensId { get; set; }

  
        public virtual CV CV { get; set; }
        public virtual Kompetens Kompetens { get; set; }
    }

}
