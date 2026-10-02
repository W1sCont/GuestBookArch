using System.ComponentModel.DataAnnotations;
namespace Guestbook.Models
{
    public class LogIn
    {
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
