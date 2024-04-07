using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AttendenceSystem.ViewModel;

namespace AttendenceSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly InstructorIRepo Instructor;
        private readonly IEmpRepo EmpRepo;
        private readonly TrackIRepo Track;

        private readonly IStudentRepo studentRepo;


        public AdminController(InstructorIRepo Repo, IEmpRepo empRepo,TrackIRepo trackrepo, IStudentRepo _studentRepo)

        {
            Instructor = Repo;
            EmpRepo = empRepo;
            Track = trackrepo;
            studentRepo = _studentRepo;


        }
        //display the instructor Data
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
        //Add Instructor
        [HttpGet]
        public IActionResult AddInstructor()
        {
            ViewBag.Track = Instructor.GetAllTracks();
            return View();
        }


        [HttpGet]
     public IActionResult ShowAllEmployees()
        
        {return View(EmpRepo.GetAllEmployees());}


        public IActionResult DeatailsEmp(int id)
        {
              
                return View(EmpRepo.GetEmployeeById(id));
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {  
            ViewBag.EmpType = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enum.GetValues(typeof(EmployeeType)));

            
            return View();
        }
        //Add Instructor
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
        //Display the details for each instructor
        [HttpGet]
        public IActionResult Details(int instructorid)
        {
            var instructor =Instructor.GetInstructor(instructorid);
            ViewBag.Tracks=Instructor.GetInstructorTrack(instructorid);        
            return View(instructor);
        }
        //edit the instructor
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
            InstructorTrackViewModel existInstructor = new InstructorTrackViewModel
            { Email = instructor.Email,
                Name = instructor.Name,
                Id = instructor.Id,
                Salary = instructor.Salary,
                HireDate = instructor.HireDate,
                Mobile = instructor.Mobile,
                Password = instructor.Password,
                RoleId= Instructor.GetRole(),
                Tracks =tracks,
            };
            return View(existInstructor);

        }
        //edit the instructor
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
        //delete instrcuctor
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

        [HttpPost]
        public IActionResult AddEmployee(Employee emp)
        {
            
            EmpRepo.AddEmployee(emp);
            return RedirectToAction("ShowAllEmployees");
        }
        [HttpGet]
        public IActionResult EditEmployee(int ?id)
        {
            if (id == null)
            {
                return BadRequest();
            }
           var emp= EmpRepo.GetEmployeeById(id.Value);
                if (emp == null)
            {
                return NotFound();
            }
              EmpRepo.GetAllEmployees();
             return View(emp);
        }

            [HttpPost]
            public IActionResult EditEmployee(Employee emp)
        {
            EmpRepo.UpdateEmployee(emp);
            return RedirectToAction("ShowAllEmployees");
        }
        public IActionResult DeleteEmployee(int id)
        {
            EmpRepo.DeleteEmployee(id);   
            return RedirectToAction("ShowAllEmployees");
        }
        //display the data of tracks
        public IActionResult DisplayTracks()
        {
            var tracks = Track.GetAllTracks();
            ViewBag.supervisors = Instructor.GetAllInstructors();
            var numberofstudent = new Dictionary<int, int>();
            foreach (var track in tracks)
            {
                var TrackStudentNumber = Track.NumberStudentRoledInTrack(track.Id);
                numberofstudent.Add(track.Id, TrackStudentNumber);
            }
            ViewBag.numberofstudent = numberofstudent;
            return View(tracks);
        }

        public IActionResult TrackDetails(int TrackId)
        {
            ViewBag.Instructors = Track.GetTrackInstructors(TrackId);
            var track = Track.GetTrackById(TrackId);
            return View(track);
        }

        public IActionResult EditTrackSupervisor(int TrackId, int SupervisorId)
        {
            var result = Track.EditSupervisor(TrackId, SupervisorId);
            if (result == 0)
            {
                TempData["FaildEdit"] = "Error occurred. Please try again later.";
            }
            else
            {
                TempData["SuccessEdit"] = "Supervisor updated successfully.";
            }
            return RedirectToAction("DisplayTracks");
        }

        public IActionResult EditTrackActive(int TrackId, bool ActiveState)
        {
            var TrackStudentNumber = Track.NumberStudentRoledInTrack(TrackId);
            if (TrackStudentNumber > 0 && ActiveState == false)
            {
                TempData["FaildEdit"] = "The track already has enrolled students, so you cannot deactivate it.";
            }
            else
            {
                Track.EditeActiveState(TrackId, ActiveState);
                TempData["SuccessEdit"] = "The active state updated successfully.";
            }
            return RedirectToAction("DisplayTracks");
        }

    }
}
