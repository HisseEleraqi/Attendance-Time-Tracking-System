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
        public AdminController(InstructorIRepo Repo, IEmpRepo empRepo)
        {
            Instructor = Repo;
            EmpRepo = empRepo;

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




    }
}
