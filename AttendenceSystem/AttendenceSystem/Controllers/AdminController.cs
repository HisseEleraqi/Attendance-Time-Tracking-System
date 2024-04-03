using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Data;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Http;
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
        DataContext db = new DataContext();



        [HttpGet]
     public IActionResult ShowAllEmployees() {return View(db.Employees.ToList());}
        public IActionResult GetEmployeeById(int id)
        {
            db.Employees.Find(id);
            return View();
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

        [HttpPost]
        public IActionResult AddEmployee(Employee emp)
        {
            db.Employees.Add(emp);
            db.SaveChanges();
            return RedirectToAction("ShowAllEmployees");
        }
        [HttpGet]
        public IActionResult EditEmployee(int ?id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Employee emp = db.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }
               db.Employees.ToList();
            return View(emp);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee emp)
        {
            db.Employees.Update(emp);
            db.SaveChanges();
            return RedirectToAction("ShowAllEmployees");
        }
        public IActionResult DeleteEmployee(int id)
        {
            Employee emp = db.Employees.Find(id);
            db.Employees.Remove(emp);
            db.SaveChanges();
            return RedirectToAction("ShowAllEmployees");
        }




    }
}
