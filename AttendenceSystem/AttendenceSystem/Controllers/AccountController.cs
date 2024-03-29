using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}
	}
}
