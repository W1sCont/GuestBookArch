using Guestbook.Models;
namespace GuestBookArch.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<MessageViews>> GetMessagesAsync();
        Task CreateUserAsync(CreateUser createUser);
        Task<bool> CheckNewLogin(CreateUser createUser);
        Task CreateMessageAsync(CreateMessage createMessage, int userId);
        Task<User> LogInAsync(LogIn log);
        Task SaveAsync();
    }
}
