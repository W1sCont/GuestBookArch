using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
namespace Guestbook.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminSalt = "A1B2C3D4E5F67890";
            string rawPassword = "admin123";

            // 2. Хешируем пароль админа тем же способом, что и у простых пользователей
            byte[] passwordBytes = Encoding.Unicode.GetBytes(adminSalt + rawPassword);
            string hash = Convert.ToHexString(SHA256.HashData(passwordBytes));

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Surname = "User",
                    Login = "admin",    
                    Password = hash,
                    Salt = adminSalt
                }
            );
            modelBuilder.Entity<Message>().HasData(
                new Message { Id = 1, UserMessage = "Перший комент", UserId = 1 },
                new Message { Id = 2, UserMessage = "Другий комент", UserId = 1 },
                new Message { Id = 3, UserMessage = "Третій комент", UserId = 1 }
            );
        }
    }
}
