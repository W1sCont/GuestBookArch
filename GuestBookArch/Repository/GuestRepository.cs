using Guestbook.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GuestBookArch.Repository
{
    public class GuestRepository(UserContext context) : IRepository
    {
        public async Task<IEnumerable<MessageViews>> GetMessagesAsync()
        {
            return await context.Messages
                .Select(m => new MessageViews
                {
                    Message = m.UserMessage,
                    Login = m.User.Login,
                    CreatedAt = m.CreatedAt
                }).OrderByDescending(m => m.CreatedAt).ToListAsync();
        }
        public async Task CreateUserAsync(CreateUser createUser)
        {
            
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            string salt = Convert.ToHexString(saltBytes);

            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + createUser.Password);
            string hash = Convert.ToHexString(SHA256.HashData(passwordBytes));

            var user = new User
            {
                Name = createUser.Name,
                Surname = createUser.Surname,
                Login = createUser.Login,
                Password = hash,
                Salt = salt
            };
            
            await context.Users.AddAsync(user);
        }

        public async Task<bool> CheckNewLogin(CreateUser createUser)
        {
            var existingUser = await context.Users
                .FirstOrDefaultAsync(u => u.Login == createUser.Login);
            return existingUser != null;
        }
        public async Task CreateMessageAsync(CreateMessage createMessage, int userId)
        {
            var message = new Message
            {
                UserMessage = createMessage.Message,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            context.Messages.Add(message);
        }
        public async Task<User> LogInAsync(LogIn log)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Login == log.Login);
            
            if (user == null) return null;

            byte[] passwordBytes = Encoding.Unicode.GetBytes(user.Salt + log.Password);
            string hash = Convert.ToHexString(SHA256.HashData(passwordBytes));

            if (hash != user.Password) return null;

            return user;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
