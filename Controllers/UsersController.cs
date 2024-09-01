using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login() { 
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var checkuser = context.Users.Where(x=> x.UserName == user.UserName && x.Password == user.Password);
            if (checkuser.Any()) { 
                return RedirectToAction("Index","Employees");
            }
            ViewBag.Error = "inalid username / password";
            return View(user);
        }
        public IActionResult InactiveUsers()
        {
            var inactiveUsers = context.Users.Where(u => u.IsActive == false).ToList();
            return View(inactiveUsers);
        }
        [HttpPost]
        public IActionResult ActivateUser(Guid id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                user.IsActive = true;
                context.SaveChanges();
            }
            return RedirectToAction(nameof(InactiveUsers));
        }
    }
}
