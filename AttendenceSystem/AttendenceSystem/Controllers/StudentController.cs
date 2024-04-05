using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
