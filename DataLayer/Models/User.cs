using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Models;

public class User : IdentityUser
{

    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public string Id { get; set; }
    public string Name { get; set; }
    public ProfileType ProfileType { get; set; }
    public string ProfilePictureUrl { get; set; }


    public virtual ICollection<UserParticipationProject> UsersParticipationsProjects { get; set; } = new List<UserParticipationProject>();

    public virtual ICollection<DataLayer.Models.Project> CreatedProjects { get; set; } = new List<DataLayer.Models.Project>();

    // En collection av mottagna meddelanden
    public virtual ICollection<Meddelande> MottagnaMeddelanden { get; set; } = new List<Meddelande>();  

    public virtual ICollection<Meddelande> SkickadeMeddelanden { get; set; } = new List<Meddelande>();


}

public enum ProfileType
{
    Private,
    Public
}
