using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models;

public class User : IdentityUser
{

    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public string Id { get; set; }
    public string Name { get; set; }
    public ProfileType ProfileType { get; set; }
    public string ProfilePictureUrl { get; set; }


    public virtual ICollection<UserParticipationProject> UsersParticipationsProjects { get; set; } = new List<UserParticipationProject>();

    public virtual ICollection<WebApplication2.Models.Project> CreatedProjects { get; set; } = new List<WebApplication2.Models.Project>();

    // En collection av mottagna meddelanden
    public virtual ICollection<Meddelande> MottagnaMeddelanden { get; set; } = new List<Meddelande>();  // dessa är nödvändiga Sayed

    public virtual ICollection<Meddelande> SkickadeMeddelanden { get; set; } = new List<Meddelande>();


    //Lista med projekt
    //public virtual ICollection<UserProject> UserProjects { get; set; }

    //Lista med konversationer kopplade till en mellantabell (UserKonversation.cs)
    //public virtual ICollection<UserKonversation> UserKonversationer { get; set; }




    // [ForeignKey(nameof(ProjectId))]
    //public virtual Projekt Projekt { get; set; }
}

public enum ProfileType
{
    Private,
    Public
}
