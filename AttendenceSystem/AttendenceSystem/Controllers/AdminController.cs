using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AddInstructor()
        {
            return View();
        }
    }
}
