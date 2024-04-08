using AttendenceSystem.IRepo;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class SecurityController : Controller
    {

        private readonly IStudentRepo _studentRepo;
        private readonly TrackIRepo _trackIRepo;
        public SecurityController(IStudentRepo studentRepo,TrackIRepo trackIRepo)
        {
         
          _studentRepo = studentRepo;
            _trackIRepo = trackIRepo;   

        } 
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult GetAllTracks()
        {
            return View();  
        }
    }
}
