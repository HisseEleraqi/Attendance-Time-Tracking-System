using Microsoft.AspNetCore.Mvc;
using AttendenceSystem.Data;
using System.Linq;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly DataContext _db=new DataContext();
        
        public IActionResult Index()
        {
            int id = 3; // Get the id from the session
            if (id == null)
                return BadRequest();
            var model = _db.Instructors.FirstOrDefault(a => a.Id == id);
            if (model == null)
                return NotFound();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var instructor = _db.Instructors.FirstOrDefault(a => a.Id == id);
            /*if (instructor == null)
            {
                return NotFound();
            }*/
            return View(instructor);
        }
        [HttpPost]
        /* public IActionResult Edit(Instructor instructor)
         {
             _db.Entry(instructor).State = EntityState.Modified;
             _db.SaveChanges();

             return View(instructor);

         }*/
        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            //get instructor from db
            var instructorFromDb = _db.Instructors.FirstOrDefault(a => a.Id == instructor.Id);
            //update the instructor (Name, Email, Password)
            instructorFromDb.Name = instructor.Name;
            instructorFromDb.Email = instructor.Email;
            instructorFromDb.Password = instructor.Password;
            //save the changes
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //instructor/student
        public IActionResult Student()
        {

            var Student = _db.Students.ToList();
            return View("student",Student);
        }


        [HttpGet]
        public IActionResult EditDegree(int id)
        {
            var student = _db.Students.FirstOrDefault(a => a.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        //instructor/EditDegree
        [HttpPost]
        public IActionResult EditDegree(Student student)
        {
            var StudentFromDb = _db.Students.FirstOrDefault(a => a.Id == student.Id);
            StudentFromDb.Degree = student.Degree;

            _db.SaveChanges();
            return RedirectToAction("Student");

        }
      

    }

}

