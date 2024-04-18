using AttendenceSystem.IRepo;

using System.Text.Json;
using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace AttendenceSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {

        private readonly IStudentRepo studentRepo;
        private readonly IUserRepo userRepo;
        private readonly InstructorIRepo instructor;

        public StudentController(IStudentRepo _studentRepo,IUserRepo _userRepo, InstructorIRepo _insRepo)

        {
            studentRepo = _studentRepo;
            userRepo= _userRepo;
            instructor = _insRepo;
        }


        public IActionResult AttendenceDetails()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            ViewBag.LateDays= studentRepo.GetStudentLateDays(userId);
            ViewBag.AbsentDays = studentRepo.GetStudentAbsentDays(userId);
            ViewBag.Degree = studentRepo.GetStudentDegrees(userId);
            ViewBag.CurrentDate = DateTime.Today.ToShortDateString();
             return View();
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


        public IActionResult PermisonDisplay()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var permisions=studentRepo.GetStudentPermision(userId);
            return View(permisions);
        }
        [HttpGet]
        public IActionResult Addpermision()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            ViewBag.StudentId = userId;
            return View();
        }
        [HttpPost]
        public IActionResult Addpermision(Permision newpermision)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            ViewBag.StudentId = userId;
            if (ModelState.IsValid)
            {
                studentRepo.Addnewpermision(newpermision);
                return RedirectToAction("PermisonDisplay");
            }
            return View(newpermision);
        }

        public IActionResult DeletePermision(int permisionId)
        {
            studentRepo.Deletpermision(permisionId);
            return RedirectToAction("PermisonDisplay");
        }





        //////////new code nyira////////////////

        public IActionResult DisplayAllStudent()
        {
            var students = studentRepo.GetAllAcceptedStudents();
            return View(students);
        }
        public IActionResult DeleteStudent(int Studentid)
        {
            try
            {
                studentRepo.DeleteStudent(Studentid);
                TempData["SuccessMessage"] = "Student deleted successfully.";


            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student.";

            }
            return RedirectToAction("DisplayAllStudent");
        }
        [HttpGet]
        public IActionResult EditStudent(int Studentid)
        {
            var student = studentRepo.GetStudentById(Studentid);
            ViewBag.tracks = studentRepo.GetTracks();
            ViewBag.studentTrack = studentRepo.GetStudentTrack(Studentid);
            return View(student);
        }
        [HttpPost]
        public IActionResult EditStudent(int Studentid, Student editedstudent)
        {
            ViewBag.tracks = studentRepo.GetTracks();
            ViewBag.studentTrack = studentRepo.GetStudentTrack(Studentid);
            if (ModelState.IsValid)
            {
                if (!instructor.ISEmailExistE(Studentid, editedstudent.Email))
                {

                    editedstudent.Id = Studentid;
                    studentRepo.EditStudent(editedstudent);
                    TempData["SuccessMessage"] = "Student edited successfully.";
                    return RedirectToAction("DisplayAllStudent");
                }

            }

            return View(editedstudent);
        }







    }
}
