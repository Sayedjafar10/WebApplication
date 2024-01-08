    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace WebApplication2.Models
    {
        public class Project
        {

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required(ErrorMessage = "Vänligen ange ett projekt namn")]
            [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = " Projekt namnet får inte innehålla specialtecken.")]
            public string Namn { get; set; }

            [Required(ErrorMessage = "Vänligen ange projekt beskrivning")]
            public string Beskrivning { get; set; }

            [Required(ErrorMessage = "Vänligen ange startdatum")]
            [DataType(DataType.DateTime, ErrorMessage = "Fel format!")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime StartTime { get; set; }

            [Required(ErrorMessage = "Vänligen ange slutdatum")]
            [DataType(DataType.DateTime, ErrorMessage = "Fel format!")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime EndTime { get; set; }
            
            public virtual ICollection<UserParticipationProject> UsersParticipationsProjects { get; set; } = new List<UserParticipationProject>();

            public string? CreatorID { get; set; }
            [ForeignKey(nameof(CreatorID))]
            public virtual User? Creator { get; set; }

        }
    }