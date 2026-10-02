using System.ComponentModel.DataAnnotations;
namespace Guestbook.Models
{
    public class CreateUser
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Display(Name = "Surname")]     
        [Required(ErrorMessage = "Surname is required.")]
        public string? Surname { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login is required.")]
        public string? Login { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm your password.")]
        public string? PasswordConfirm { get; set; }
    }
}
