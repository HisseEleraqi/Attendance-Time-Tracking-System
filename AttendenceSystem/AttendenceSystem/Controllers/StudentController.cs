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
        private readonly IHostEnvironment _env;

        private readonly DataContext context = new DataContext();

        public StudentController(IStudentRepo _studentRepo, IAttendance attendance, IHostEnvironment env)

        {
            studentRepo = _studentRepo;
            _attendance = attendance;
            _env = env;

        }


        public IActionResult AttendenceDetails()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            ViewBag.LateDays = studentRepo.GetStudentLateDays(userId);
            ViewBag.AbsentDays = studentRepo.GetStudentAbsentDays(userId);
            ViewBag.Degree = studentRepo.GetStudentDegrees(userId);
            ViewBag.CurrentDate = DateTime.Today.ToShortDateString();
            return View();
        }

        public IActionResult Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);

            var user = studentRepo.GetStudentById(userId);

            return View(user);

        }

        public IActionResult StudentScdule()
        {
            //var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //var userId = int.Parse(userIdClaim);
            //var user=studentRepo.StudentSchedule(userId);
            return View();

        }




        //student attendance report ****//

        // filtering with date and attendance state
        [HttpPost]
        public IActionResult FilterAttendanceReport(DateOnly startDate, DateOnly endDate, AttendanceEnum attendanceStatus)
        {
            List<Attendence> filteredAttendance = new List<Attendence>();

            switch (attendanceStatus)
            {
                case AttendanceEnum.IsApsent:
                    filteredAttendance = _attendance.GetAbsentStudents(startDate, endDate);
                    break;

                case AttendanceEnum.IsLate:
                    filteredAttendance = _attendance.GetLateStudents(startDate, endDate);
                    break;

                case AttendanceEnum.IsPresent:
                    filteredAttendance = _attendance.GetPresentStudents(startDate, endDate);
                    break;

                default:

                    break;
            }

           
            List<AttendanceReportResponse> reportResult = new List<AttendanceReportResponse>();
           foreach(var attendance in filteredAttendance)
            {

                var  student = context.Students.AsNoTracking().Where(a => a.Id == attendance.UserId).FirstOrDefault();
               
                reportResult.Add(new AttendanceReportResponse() {AttendanceStatue = attendanceStatus , InDate = attendance.Date, OutTime = attendance.OutTime , InTime = attendance.InTime  , Grade = student.Degree  , IsAbsent = attendance.IsAbsent , IsLate = attendance.IsLate ,  IsPresent = attendance.IsPresent  , userName = student.Name});

            }



          return View("PrintStudentReport22" , reportResult);
        }


        public IActionResult PrintStudentReport22([FromRoute] int Id)
        {
            //var Students = context.Attendences.AsNoTracking().Include(a => a.User).Where(a => a.TrackId == Id);
            var Students = _attendance.GetAttendencesTrackId(Id, UserTypeEnum.Student);
            List<AttendanceReportResponse> reportResult = new List<AttendanceReportResponse>();
            foreach (var attendance in Students)
            {

                var student = context.Students.AsNoTracking().Where(a => a.Id == attendance.UserId).FirstOrDefault();

                reportResult.Add(new AttendanceReportResponse() {InDate = attendance.Date, OutTime = attendance.OutTime, InTime = attendance.InTime, Grade = student.Degree, IsAbsent = attendance.IsAbsent, IsLate = attendance.IsLate, IsPresent = attendance.IsLate, userName = student.Name });

            }

      
            return View(reportResult);

        }


        [HttpPost("ExportToExcel")]
        public ActionResult ExportToExcel(int ID)
        {
            try
            {

                //var Students = _trackIRepo.GetStudentsByTrackId(ID);
                var Students = _attendance.GetAttendencesTrackId(ID, UserTypeEnum.Student);

                List<string> Header = new List<string>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(new FileInfo("StudentReport.xlsx")))
                {

                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    Header.Add("Date");
                    Header.Add("InTime");
                    Header.Add("OutTime");
                    Header.Add("Name");

                    var headerRow = new List<string[]>()
                    {
                      Header.ToArray()
                    };

                    string headerRange = "A1:" + Char.ConvertFromUtf32(Header[0].Length + 64) + "1";
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    int InsertRowIndex = 1;
                    foreach (var item in Students)
                    {
                        InsertRowIndex++;
                        worksheet.Cells[string.Format("A{0}", InsertRowIndex)].Value = item.Date;
                        worksheet.Cells[string.Format("B{0}", InsertRowIndex)].Value = item.InTime;
                        worksheet.Cells[string.Format("C{0}", InsertRowIndex)].Value = item.OutTime;
                        worksheet.Cells[string.Format("D{0}", InsertRowIndex)].Value = item.User?.Name;

                    }

                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StudentReport.xlsx");
                    }
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        //****************//





        public IActionResult PermisonDisplay()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var permisions = studentRepo.GetStudentPermision(userId);
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
        public ActionResult DownloadStudentExcel()
        {
            string path = Path.Combine(_env.ContentRootPath + "/wwwroot/ExcelTemplate/UploadBulkStudent.xlsx");

            if (System.IO.File.Exists(path))
                return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadBulkStudent.xlsx");

            else
                return RedirectToAction("UploadBulkStudent");
            //return NotFound();

        }

        [HttpPost]
        public IActionResult UploadBulkStudent(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                // Handle empty file error
                return RedirectToAction("Error");
            }
            List< Student > StudentList = new List<Student>();
            string exttension = System.IO.Path.GetExtension(file.FileName);
            if (exttension == ".xlsx")
            {
              using (var package = new ExcelPackage(file.OpenReadStream()))
              {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
                    int rowCount = worksheet.Dimension.Rows;
                    // Process the data

                    string userName, userEmail, userMobile, studentSpec, studentFaculty, studentUniversity = "";
                    int studentDegree, graduation = default ,userPassword, studentTrackId;

                    if (rowCount > 1)
                    {
                        
                        try
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                // Access the cell values using the row and column indexes
                                userName = worksheet.Cells[row, 1].Value?.ToString();
                                userEmail = worksheet.Cells[row, 2].Value?.ToString();
                                userMobile = worksheet.Cells[row, 3].Value?.ToString();



                                studentDegree = (int)Convert.ToDouble(worksheet.Cells[row, 4].Value); //int.Parse(worksheet.Cells[row, 5].Value);
                                studentSpec = worksheet.Cells[row, 5].Value?.ToString();
                                graduation = (int)Convert.ToDouble(worksheet.Cells[row, 6].Value); //int.Parse(worksheet.Cells[row, 6].Value?.ToString());
                                studentFaculty = worksheet.Cells[row, 7].Value?.ToString();
                                studentUniversity = worksheet.Cells[row, 8].Value?.ToString();
                                //password
                                userPassword = int.Parse(worksheet.Cells[row, 9].Value?.ToString());
                                //track id
                                studentTrackId = int.Parse(worksheet.Cells[row, 10].Value?.ToString());


                                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail)
                                    || string.IsNullOrEmpty(userMobile) || string.IsNullOrEmpty(studentSpec)
                                    || string.IsNullOrEmpty(studentFaculty) || string.IsNullOrEmpty(studentUniversity)
                                    || (studentDegree == null || studentDegree == 0)
                                    || (graduation == null || graduation == 0))
                                {
                                    return BadRequest("fileIsEmpty");
                                }

                                // var studentTrackId= int.Parse(worksheet.Cells[row, 11].Value?.ToString());

                                Student student = new Student() { Name = userName, Email = userEmail, Password = userPassword.ToString(), Mobile = userMobile, Degree = studentDegree, Specification = studentSpec, GraduationYear = graduation, Faculty = studentFaculty, University = studentUniversity, TrackID = studentTrackId };
                                StudentList.Add(student);
                                //studentRepo.AddStudent(student);

                            }
                            context.Students.AddRange(StudentList);
                        }
                        catch(Exception ex)
                        {
                            ViewBag.Error = "Error saving the data!";
                            return View("AdminError");
                        }

                    }
                    else
                        return BadRequest("fileIsEmpty");
                }
            }
            else
                return BadRequest("file Not Support");
            // Redirect to a success page or return a JSON response indicating success
            context.SaveChanges();

            return RedirectToAction("Index", "Student");
        }




    }
}
