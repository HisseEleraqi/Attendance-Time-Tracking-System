using AttendenceSystem.CustomFilter;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using AttendenceSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    [AuthFilter]

    public class StudentAffairController : Controller
    {
        private readonly IStudentService studentService;
        private readonly IStudentRepo studentRepo;
        private readonly TrackIRepo trackIRepo;

        public StudentAffairController(IStudentService _studentService, IStudentRepo _studentRepo, TrackIRepo _trackIRepo)
        {
            studentService = _studentService;
            studentRepo = _studentRepo;
            trackIRepo = _trackIRepo;
        }
        public IActionResult Attendance()
        {
            List<Track> tracks = trackIRepo.GetActiveTracks();
            return View(tracks);
        }
        [HttpGet]
        public IActionResult GetStudentDetails(int id)
        {
            // Retrieve student details based on the selected name
            var student = studentRepo.GetStudentById(id);

            if (student != null)
            {
                return PartialView("_StudentDetailsPartial", student);
            }

            return NotFound();
        }
        [HttpGet]
        public IActionResult StudentAttendance(int studentId)
        {
            StudentAttendanceViewModel viewModel = studentService.GetStudentAttendance(3);
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult StudentAttendanceAtTrack(int trackId, DateOnly date)
        {
            DateOnly date1 = new DateOnly(2024, 1, 4);
            List<StudentAttendanceViewModel> viewModels = studentService.GetTrackAttendancedate(trackId, date);

            return PartialView("StudentAttendanceAtTrack", viewModels);
            //return Json(viewModels);
        }
        [HttpGet]
        public IActionResult EditDegree(int id)
        {
            Student student = studentRepo.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student); // Pass the student model to the view
        }

        [HttpPost]
      
        public IActionResult EditDegree(int id, Student viewModel)
        {
           

            studentRepo.UpdateStudentDegree(id, viewModel.Degree);

            return RedirectToAction("Attendance", "StudentAffair");
        }



    }

} 


