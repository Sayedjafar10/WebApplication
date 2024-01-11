using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{   

    // M:N samband mellan User och Projekt
    public class UserParticipationProject
    {
        public UserParticipationProject()
        {
            // empty
        }

        
        public string UserID { get; set; }

        public int ProjectID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }

		

	}
}
