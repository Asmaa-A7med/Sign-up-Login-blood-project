using System.ComponentModel.DataAnnotations;

namespace loginProject.Models
{
    public class UserRegisterRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.PhoneNumber)]
        [Required, MaxLength(14, ErrorMessage = "The National ID must be 14 numbers!"), MinLength(14, ErrorMessage = "The National ID must be 14 numbers!")]
        public string NationalID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string BloodBank { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
