using AttendenceSystem.Data;
using AttendenceSystem.IRepo;


using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Claims;


namespace AttendenceSystem.Controllers
{
    public class StudentController : Controller
    {

        private readonly IStudentRepo studentRepo;
        private readonly IAttendance _attendance;

        private readonly DataContext context = new DataContext();

        public StudentController(IStudentRepo _studentRepo, IAttendance attendance)

        {
            studentRepo = _studentRepo;
            _attendance = attendance;

        }


        public IActionResult AttendenceDetails()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            ViewBag.LateDays= studentRepo.GetStudentLateDays(userId);
            ViewBag.AbsentDays = studentRepo.GetStudentAbsentDays(userId);
            ViewBag.Degree = studentRepo.GetStudentDegrees(userId);
            ViewBag.CurrentDate = DateTime.Today.ToShortDateString();
             return View();
        }

        public IActionResult  Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            var user = studentRepo.GetStudentById(userId);

            return View(user);

        }

        public IActionResult StudentScdule()
        {
           var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
           var userId = int.Parse(userIdClaim);
           var user=studentRepo.StudentSchedule(userId);
           return View();
           
        }
        public IActionResult PrintStudentReport22([FromRoute] int Id)
        {
            //var Students = context.Attendences.AsNoTracking().Include(a => a.User).Where(a => a.TrackId == Id);
            var Students = _attendance.GetAttendencesTrackId(Id, UserTypeEnum.Student);
            ViewBag.ID = Id;
            return View(Students);

        }

        public IActionResult PermisonDisplay()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var permisions=studentRepo.GetStudentPermision(userId);
            return View(permisions);
        }
        [HttpGet]
        public IActionResult Addpermision()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            ViewBag.StudentId = userId;
            return View();
        }
        [HttpPost]
        public IActionResult Addpermision(Permision newpermision)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            ViewBag.StudentId = userId;
            if (ModelState.IsValid)
            {
                studentRepo.Addnewpermision(newpermision);
                return RedirectToAction("PermisonDisplay");
            }
            return View(newpermision);
        }

        public IActionResult DeletePermision(int permisionId)
        {
            studentRepo.Deletpermision(permisionId);
            return RedirectToAction("PermisonDisplay");
        }


        [HttpGet]
        public IActionResult UploadBulkStudent()
        {
            return View();
        }



        [HttpPost]
        public IActionResult UploadBulkStudent(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length <= 0)
            {
                // Handle empty file error
                return RedirectToAction("Error");
            }

            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

                // Process the data
                for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                {
                    // Access the cell values using the row and column indexes
                    var userName = worksheet.Cells[row, 1].Value?.ToString();
                    var userPassword = worksheet.Cells[row, 2].Value?.ToString();
                    var userEmail= worksheet.Cells[row, 3].Value?.ToString();
                    var userMobile = worksheet.Cells[row, 4].Value?.ToString();


                    //studentRepo.AddUser(user);

                    var studentDegree = int.Parse(worksheet.Cells[row, 5].Value?.ToString());
                    var studentSpec = worksheet.Cells[row, 6].Value?.ToString();
                    var graduation = int.Parse(worksheet.Cells[row, 7].Value?.ToString());
                    var studentFaculty= worksheet.Cells[row, 8].Value?.ToString();
                    var studentUniversity = worksheet.Cells[row, 9].Value?.ToString();
                    var studentIsAccepted= bool.Parse(worksheet.Cells[row, 10].Value?.ToString());
                    var studentTrackId= int.Parse(worksheet.Cells[row, 11].Value?.ToString());

                    Student student =  new Student() { Name = userName  , Password = userPassword , Email = userEmail , Mobile = userMobile , Degree = studentDegree , Specification = studentSpec , GraduationYear = graduation , Faculty = studentFaculty , University = studentUniversity  , IsAccepted = studentIsAccepted , TrackID = studentTrackId};
                      studentRepo.AddStudent(student);

                }
            }

            // Redirect to a success page or return a JSON response indicating success
            return RedirectToAction("Index" , "Student");
        }




    }
}
