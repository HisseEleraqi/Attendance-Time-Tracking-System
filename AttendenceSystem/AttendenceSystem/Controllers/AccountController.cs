using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using AttendenceSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace AttendenceSystem.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepo loginRepo;
        public AccountController(IAccountRepo _loginRepo)
        {
            loginRepo = _loginRepo;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = loginRepo.GetUser(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email or password");
                return View(model);
            }

            Claim claim1;
            Claim claim2;
            Claim claim3;
            Claim claim4;

            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claim1 = new Claim(ClaimTypes.Email, user.Email);
            claim2 = new Claim(ClaimTypes.Name, user.Name);
            claim3 = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            string imageUrl = string.IsNullOrEmpty(user?.Img) ? "noUser.jpg" : user.Img;
            claim4 = new Claim("ImageUrl", imageUrl);
            var userRole = user.Roles; // Assuming the role is stored in the user object

            var userRoles = user.Roles.Select(r => r.RoleName).ToList(); // Get all role names associated with the user

            if (userRoles.Contains("Student"))
            {
                return RedirectToAction("Index", "StudentDashboard");
            }
            else if (userRoles.Contains("Supervisor") && userRoles.Contains("Instructor"))
            {
                return RedirectToAction("Index", "SupervisorDashboard");
            }
            else if (userRoles.Contains("Instructor"))
            {
                return RedirectToAction("Index", "InstructorDashboard");
            }
            else if (userRoles.Contains("Admin"))
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            else if (userRoles.Contains("Security"))
            {
                return RedirectToAction("Index", "SecurityDashboard");
            }
            else if (userRoles.Contains("Student_affairs"))
            {
                return RedirectToAction("Index", "Student_affairsDashboard");
            }
            
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> LogOut()

        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
