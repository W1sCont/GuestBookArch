namespace Guestbook.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? UserMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
