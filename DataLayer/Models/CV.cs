using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Models;

namespace DataLayer.Models
{
    public class CV
    {
        //Saxat från kravspec: Information som ska finnas med är minst; Kompetenser, Utbildningar, Tidigare erfarenhet
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id{ get; set; }
        public virtual ICollection<CVUtbildning>? CVUtbildningar { get; set; } 
        public virtual ICollection<CVErfarenhet>? CVErfarenheter { get; set; }
        public virtual ICollection<CVKompetens>? CVKompetenser { get; set; }  


        public virtual ICollection<CVImage> CVImages { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }



    }
}



