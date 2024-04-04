using AttendenceSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class InstructorController : Controller
    {
        DataContext db = new DataContext();
        /* public IActionResult ViewProfile(int id)
         {
             var instructor = db.Instructors.Find(id);
             ViewBag.Instructor = instructor;

             return View();
         }*/
        [HttpGet("/Profile")]
        public IActionResult Profile()
        {
            int id = 2; // Get the id from the session
            if (id == null)
                return BadRequest();
            var model = db.Instructors.FirstOrDefault(a => a.Id == id);
            if (model == null)
                return NotFound();
            return View("Details", model);
        }
       /* public IActionResult Details(int id)
        {
            if (id == null)
                return BadRequest();
            var model = db.Instructors.FirstOrDefault(a => a.Id == id);
            if (model == null)
                return NotFound();
            return View(model);
        }*/

    }
}

