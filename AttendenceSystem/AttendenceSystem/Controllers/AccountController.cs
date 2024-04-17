using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using AttendenceSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using AttendenceSystem.IRepo;


namespace AttendenceSystem.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepo loginRepo;

       
        
       private readonly TrackIRepo trackIRepo;
        private readonly IStudentRepo studentRepo;

        public AccountController(IAccountRepo _loginRepo, IStudentRepo _studentRepo,TrackIRepo _trackIRepo)
        {
            loginRepo = _loginRepo;
            
            studentRepo=_studentRepo;
            trackIRepo = _trackIRepo;
        }
       

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel model)
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
            var userRoles2 = user.Roles.Select(r => r.Role); // Assuming RoleName property holds the role name

            foreach (var role in userRoles2)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role.RoleName); // Assuming RoleName property holds the role name
                claimsIdentity.AddClaim(roleClaim);
            }


            claimsIdentity.AddClaim(claim1);
            claimsIdentity.AddClaim(claim2);
            claimsIdentity.AddClaim(claim3);
            //claimsIdentity.AddClaim(claim4);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            var userRole = user.Roles; // Assuming the role is stored in the user object
           
            var userRoles = user.Roles.Select(r => r.Role.RoleName).ToList(); // Get all role names associated with the user

            if (userRoles.Contains("Student"))
            {

                var student = studentRepo.GetStudentById(user.Id); // Implement a method to retrieve student by email
                if (student != null && student.IsAccepted)
                {
                    return RedirectToAction("Index", "Student"); // Redirect to student dashboard
                }
                else
                {
                    return RedirectToAction("PendingRegistration", "Account"); // Redirect to a page indicating pending registration
                }

            }
            else if (userRoles.Contains("Supervisor") && userRoles.Contains("Instructor"))
            {

                return RedirectToAction("Index", "Instructor");

            }
            else if (userRoles.Contains("Instructor"))
            {
                return RedirectToAction("Index", "Instructor");
            }
            else if (userRoles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (userRoles.Contains("Student_affairs"))
            {
                if (userRoles.Contains("Security"))
                {
                    return RedirectToAction("Index", "SecurityDashboard");
                }
                else
                {
                    return RedirectToAction("Attendance", "StudentAffair");
                }
            }
            

            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Signup()
        {
            // Fetch active tracks from the database and pass them to the view
            ViewBag.ActiveTracks = trackIRepo.GetActiveTracks() ?? new List<Track>();
            return View();
        }

        // POST: /Account/Signup
        [HttpPost]
        public async Task<IActionResult> Signup(Student student)
        {
            if (ModelState.IsValid)
            {
                studentRepo.AddStudent(student);

                // Redirect to a success page or do something else
                return RedirectToAction("Login", "Account");
            }

            // If model validation fails, reload the signup page with validation errors
            ViewBag.ActiveTracks = trackIRepo.GetActiveTracks() ?? new List<Track>();
            return View(student);
        }

        public async Task<IActionResult> LogOut()

        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult PendingRegistration()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            
            return View();
        }

    }
}
 


