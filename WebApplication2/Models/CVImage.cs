namespace WebApplication2.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace WebApplication2.Models
    {
        public class CVImage
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            public string FileName { get; set; }

            // ForeignKey för att länka till CV
            [Required]
            public string CVId { get; set; }

            // Navigationsegenskap till CV
            [ForeignKey("CVId")]
            public virtual CV CV { get; set; }
        }
    }

}
