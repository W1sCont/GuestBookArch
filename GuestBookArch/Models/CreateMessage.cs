using System.ComponentModel.DataAnnotations;
namespace Guestbook.Models
{
    public class CreateMessage
    {
        [Required(ErrorMessage = "Message is required.")]
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
