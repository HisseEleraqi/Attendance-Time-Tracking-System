using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using AttendenceSystem.ViewModel;

namespace AttendenceSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly InstructorIRepo Instructor;
        public AdminController(InstructorIRepo Repo)
        {
            Instructor=Repo;
        }
        public IActionResult Index()
        {
           var instructors= Instructor.GetAllInstructors();
           return View(instructors);
        }
        [HttpGet]
        public IActionResult AddInstructor()
        {
            ViewBag.Track = Instructor.GetAllTracks();
            return View();
        }
        [HttpPost]
        public IActionResult AddInstructor(InstructorTrackViewModel instructor)
        {
            ViewBag.Track = Instructor.GetAllTracks();
            if (ModelState.IsValid)
            {
                if(!Instructor.ISEmailExist(instructor.Email))
                {
                    Instructor.AddInstructor(instructor);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Email Is Already Exist";
                    return View();
                }
            }
            return View(instructor);
        }
        [HttpGet]
        public IActionResult Details(int instructorid)
        {
            var instructor =Instructor.GetInstructor(instructorid);
            ViewBag.Tracks=Instructor.GetInstructorTrack(instructorid);        
            return View(instructor);
        }
    }
}
