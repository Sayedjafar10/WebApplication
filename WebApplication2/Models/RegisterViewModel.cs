using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
	public class RegisterViewModel
	{
		public int Id { get; set; }
		[Required]
		[EmailAddress]

		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
		public string ConfirmPassword { get; set; }


		[Required]
		public string Name { get; set; }


        [Required]
        public ProfileType ProfileType { get; set; }

        // Andra egenskaper efter behov
    }
}
