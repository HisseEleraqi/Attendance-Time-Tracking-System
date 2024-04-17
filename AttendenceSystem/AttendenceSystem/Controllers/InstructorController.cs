using Microsoft.AspNetCore.Mvc;
using AttendenceSystem.Data;
using System.Linq;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AttendenceSystem.Controllers
{
    //Authorize For Instructor Or SuperVisor
    [Authorize(Roles = "Instructor, Supervisor")]
    public class InstructorController : Controller
    {
        private readonly DataContext _db=new DataContext();
        private int userId;
        //list of String of Roles
        private List<string> roles;

        public bool IsSuperVisor()
        {
            return roles.Contains("Supervisor");
        }

        public IActionResult Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();
            var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var model = _db.Instructors.FirstOrDefault(a => a.Id == userId);
            if (model == null)
                return NotFound();

            // Add IsSuperVisor to ViewBag
            ViewBag.IsSuperVisor = IsSuperVisor();

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();

            var instructor = _db.Instructors.FirstOrDefault(a => a.Id == userId);
            return View(instructor);
        }

        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();

            //get instructor from db
            var instructorFromDb = _db.Instructors.FirstOrDefault(a => a.Id == instructor.Id);
            //update the instructor (Name, Email, Password)
            instructorFromDb.Name = instructor.Name;
            instructorFromDb.Email = instructor.Email;
            instructorFromDb.Password = instructor.Password;
            //save the changes
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Student()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();

            // Only if Roles Contain SuperVisior
            if (!roles.Contains("Supervisor"))
            {
                //return Access Denied
                return NotFound();
               
            }
            int SuperVisorTrackId= _db.Tracks.FirstOrDefault(a => a.SupervisorId == userId).Id;
            var Student = _db.Students.Where(a => a.TrackID == SuperVisorTrackId).ToList();
            return View("student",Student);
        }

        //Only Supervisor
        [Authorize(Roles = "Supervisor")]
        [HttpGet]
        public IActionResult EditDegree(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();

            var student = _db.Students.FirstOrDefault(a => a.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        //Only Supervisor
        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        public IActionResult EditDegree(Student student)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(userIdClaim);
            // Get User with userId
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            // Get Roles of User
            roles = user.Roles.Select(r => r.Role.RoleName).ToList();

            var StudentFromDb = _db.Students.FirstOrDefault(a => a.Id == student.Id);
            StudentFromDb.Degree = student.Degree;

            _db.SaveChanges();
            return RedirectToAction("Student");

        }
      

    }

}

