using AttendenceSystem.Data;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendenceSystem.Controllers
{
    public class AdminController : Controller
    {
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
