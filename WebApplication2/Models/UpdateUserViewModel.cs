using System.ComponentModel.DataAnnotations;

public class UpdateUserViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "E-post")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Namn")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Profiltyp")]
    public ProfileType ProfileType { get; set; }

    // Lägg till andra fält som behövs för uppdatering
}
