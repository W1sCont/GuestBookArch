using Guestbook.Models;
using GuestBookArch.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Guestbook.Controllers
{
    public class HomeController(IRepository repo) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await repo.GetMessagesAsync();
            return View(result);
        }
        public IActionResult CreateUser() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Name,Surname,Login,Password,PasswordConfirm")] CreateUser createUser)
        {
            if (!ModelState.IsValid) return View(createUser);

            if (string.IsNullOrEmpty(createUser.Login) || string.IsNullOrEmpty(createUser.Password))
            {
                ModelState.AddModelError(string.Empty, "Логін і пароль не можуть бути порожніми.");
                return View(createUser);
            }

            if(createUser.Password != createUser.PasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Паролі не співпадають.");
                return View(createUser);
            }

            if (await repo.CheckNewLogin(createUser))
            {
                ModelState.AddModelError(string.Empty, "Користувач з таким логіном вже існує.");
                return View(createUser);
            }

            await repo.CreateUserAsync(createUser);
            await repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([FromForm] LogIn log)
        {
            if (!ModelState.IsValid) return View(log);

            var user = await repo.LogInAsync(log);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
                return View(log);
            }

            HttpContext.Session.SetString("login", user.Login);
            HttpContext.Session.SetString("Name", user.Name);
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateMessage() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage([Bind("Message")] CreateMessage createMessage)
        {
            if (!ModelState.IsValid) return View(createMessage);

            if (string.IsNullOrEmpty(createMessage.Message)) return View(createMessage);

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return View(createMessage);
            else
            {
                await repo.CreateMessageAsync(createMessage, userId.Value);
                await repo.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}