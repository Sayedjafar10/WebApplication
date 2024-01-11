using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Erfarenhet
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Beskrivning { get; set; }

        

        // Navigeringsegenskap för många-till-många-relation till CV
        public virtual ICollection<CVErfarenhet> CVErfarenheter { get; set; }
    }

}
