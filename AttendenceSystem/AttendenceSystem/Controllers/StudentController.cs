using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendenceSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;

        public StudentController(IStudentRepo _studentRepo)

        {
            studentRepo = _studentRepo;
        }
        public IActionResult  Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            var user = studentRepo.GetStudentById(userId);

            return View(user);

        }

        public IActionResult StudentScdule()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
           var user=studentRepo.StudentSchedule(userId);
           return View(user);
           
        }


  


        

    }
}
