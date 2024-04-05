using Microsoft.AspNetCore.Mvc;
using AttendenceSystem.Data;
using System.Linq;

namespace AttendenceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly DataContext _db=new DataContext();

      
        public IActionResult Profile()
        {
            int id = 2; // Get the id from the session
            if (id == null)
                return BadRequest();
            var model = _db.Instructors.FirstOrDefault(a => a.Id == id);
            if (model == null)
                return NotFound();
            return View(model);
        }
    }
}
