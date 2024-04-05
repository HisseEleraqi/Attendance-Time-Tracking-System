using AttendenceSystem.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class StudentController : Controller
    {
        StudentIRepo Student;
        public StudentController(StudentIRepo repo) 
        { 
            Student= repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AttendenceDetails(int StudentID)
        {
            ViewBag.LateDays= Student.GetStudentLateDays(6);
            ViewBag.AbsentDays = Student.GetStudentAbsentDays(6);
            ViewBag.Degree = Student.GetStudentDegrees(6);
            ViewBag.CurrentDate = DateTime.Today.ToShortDateString();
             return View();
        }
    }
}
