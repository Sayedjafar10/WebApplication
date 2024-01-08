using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
	public class LoginViewModel
    {
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Kom ihåg mig")]
		public bool RememberMe { get; set; }
	}

}
