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
            var instructors = Instructor.GetAllInstructors(); 
            var instructorDetails = new List<InstructorDetails>();

            foreach (var instructor in instructors)
            {
                var lateDays = Instructor.GetLateDays(instructor.Id);
                var absentDays = Instructor.GetAbsentDays(instructor.Id);

                var instructorDetail = new InstructorDetails
                {
                    Id = instructor.Id,
                    Name = instructor.Name,
                    Email=instructor.Email,
                    Salary=instructor.Salary,
                    HireDate=instructor.HireDate,
                    LateDays = lateDays,
                    AbsentDays = absentDays
                };

                instructorDetails.Add(instructorDetail);
            }
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData.ContainsKey("DeleteErrorMessage"))
            {
                ViewBag.DeleteErrorMessage = TempData["DeleteErrorMessage"];
            }
            return View(instructorDetails);
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
        [HttpGet]
        public IActionResult Edit(int instructorid)
        {
            var instructor = Instructor.GetInstructor(instructorid);
            ViewBag.Tracks = Instructor.GetTracksExceptInstructorTrack(instructorid);
            ViewBag.InstructorTrack = Instructor.GetInstructorTrack(instructorid);
            List<int> tracks = new List<int>();
            foreach(var item in ViewBag.InstructorTrack)
            {
                tracks.Add(item.Id);
            }
            InstructorTrackViewModel existInstructor =new InstructorTrackViewModel 
            {   Email = instructor.Email,
                Name = instructor.Name,
                Id=instructor.Id,
                Salary=instructor.Salary,
                HireDate=instructor.HireDate,
                Mobile=instructor.Mobile,
                Password=instructor.Password,
                RoleId=instructor.RoleId,
                Tracks=tracks,
            };
            return View(existInstructor);

        }
        [HttpPost]
        public IActionResult Edit(int instructorid,InstructorTrackViewModel instructor)
        {
            ViewBag.Tracks = Instructor.GetTracksExceptInstructorTrack(instructorid);
            ViewBag.InstructorTrack = Instructor.GetInstructorTrack(instructorid);
            if (ModelState.IsValid)
            {
                if (!Instructor.ISEmailExistE(instructorid, instructor.Email))
                {
                    Instructor.EditInstructor(instructorid,instructor);
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
        public IActionResult Delete(int instructorid)
        {
            var instructor = Instructor.GetInstructor(instructorid);
            if (instructor.TrackInstructors.Any())
            {
                TempData["DeleteErrorMessage"] = "Instructor has tracks and cannot be deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                Instructor.DeleteInstructor(instructorid);
                TempData["SuccessMessage"] = "Instructor deleted successfully.";
                return RedirectToAction("Index");
            }
           
        }

    }
}
