using AspNetCore.Reporting;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace AttendenceSystem.Controllers
{
    public class SecurityController : Controller
    {

        private readonly IStudentRepo _studentRepo;
        private readonly TrackIRepo _trackIRepo;
        private readonly IAttendance _attendance;
        private readonly InstructorIRepo _instructorIRepo;

        public SecurityController(IStudentRepo studentRepo, TrackIRepo trackIRepo , IAttendance attendance , InstructorIRepo instructorIRepo)
        {

            _studentRepo = studentRepo;
             _attendance = attendance;
            _trackIRepo = trackIRepo;
            _instructorIRepo= instructorIRepo;
        }
        public IActionResult Index()
        {
            return View();
        }


        // GetAllTracks for student attendance

        public IActionResult GetAllTracks()
        {

            var tracks = _trackIRepo.GetAllTracks();
            return View(tracks);
        }



        // get Student by track id for attendance

        [HttpGet("GetStudentByTrackID/{id}")]
        public IActionResult GetStudentByTrackID([FromRoute] int id)
        {
            var students = _trackIRepo.GetStudentsByTrackId(id);
            return View(students);
        }


        // Student confirmation attendance


        [HttpPost]
        public IActionResult ConfirmStudentAttendace([FromRoute]int Id)
        {
            DateTime studentDate = DateTime.Now;
            DateTime dateOnly = studentDate.Date;
            string studentTime = studentDate.ToString("hh:mm:ss");
            string correctTime = String.Format("09:00:00");

            Attendence studentAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId =Id};


            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {
              
                studentAttendance.IsPresent = true;
            }
            else if (comparison > 0)
            {
                studentAttendance.IsLate = true;
            }
            else
            {
                studentAttendance.IsPresent = true;
            }
           _attendance.ConfirmStudentAttendance(studentAttendance);

            return RedirectToAction("GetAllTracks");
            

        }



        // Get Tracks Instructors works in
        public IActionResult GetAllInstructorsTracks()
        {

            var tracks = _trackIRepo.GetAllTracks();
            return View(tracks);
        }

        // get Instructors in track for attendance

       // [HttpGet("GetTrackInstructors/{id}")]
        public IActionResult GetTrackInstructors([FromRoute] int id)
        {
            var instructors = _trackIRepo.GetTrackInstructors(id);
            return View(instructors);
        }


        // Instructor confirmation attendance

        [HttpPost]
        public IActionResult ConfirmInstructorAttendance([FromRoute] int Id)
        {
            DateTime instructortDate = DateTime.Now;
            DateTime dateOnly = instructortDate.Date;
            string studentTime = instructortDate.ToString("hh:mm:ss");
            string correctTime = String.Format("09:00:00");

            Attendence instructorAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId = Id };


            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {

                instructorAttendance.IsPresent = true;
            }
            else if (comparison > 0)
            {
                instructorAttendance.IsLate = true;
            }
            else
            {
                instructorAttendance.IsPresent = true;
            }
            _attendance.ConfirmStudentAttendance(instructorAttendance);

            return RedirectToAction("GetInstructors");


        }


    }


    }



