using AspNetCore.Reporting;
using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OfficeOpenXml;

namespace AttendenceSystem.Controllers
{
    public class SecurityController : Controller
    {

        private readonly IStudentRepo _studentRepo;
        private readonly TrackIRepo _trackIRepo;
        private readonly IAttendance _attendance;
        private readonly InstructorIRepo _instructorIRepo;
        private readonly DataContext context = new DataContext();
        public SecurityController(IStudentRepo studentRepo, TrackIRepo trackIRepo , IAttendance attendance , InstructorIRepo instructorIRepo)
        {

            _studentRepo = studentRepo;
             _attendance = attendance;
            _trackIRepo = trackIRepo;
            _instructorIRepo= instructorIRepo;
        }
        public IActionResult Index()
        {
            return View();
        }


        // GetAllTracks for student attendance

        public IActionResult GetAllTracks()
        {

            var tracks = _trackIRepo.GetAllTracks();
            return View(tracks);
        }



        // get Student by track id for attendance

        [HttpGet("GetStudentByTrackID/{id}")]
        public IActionResult GetStudentByTrackID([FromRoute] int id)
        {
            var students = _trackIRepo.GetStudentsByTrackId(id);
            ViewBag.ID = id;
            return View(students);
        }

        [HttpPost("ExportToExcel/{ID}")]
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
                    Header.Add("Id");

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

        // Student confirmation attendance


        [HttpPost]
        public IActionResult ConfirmStudentAttendace([FromRoute]int Id)
        {
            DateTime studentDate = DateTime.Now;
            DateTime dateOnly = studentDate.Date;
            string studentTime = studentDate.ToString("hh:mm:ss");
            string correctTime = String.Format("09:00:00");

            Attendence studentAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId =Id,UserType= UserTypeEnum.Student };


            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {
              
                studentAttendance.IsPresent = true;
            }
            else if (comparison > 0)
            {
                studentAttendance.IsLate = true;
            }
            else
            {
                studentAttendance.IsPresent = true;
            }
           _attendance.ConfirmStudentAttendance(studentAttendance);

            return RedirectToAction("GetAllTracks");
            

        }



        // Get Tracks Instructors works in
        public IActionResult GetAllInstructorsTracks()
        {

            var tracks = _trackIRepo.GetAllTracks();
            return View(tracks);
        }

        // get Instructors in track for attendance

       // [HttpGet("GetTrackInstructors/{id}")]
        public IActionResult GetTrackInstructors([FromRoute] int id)
        {
            var instructors = _trackIRepo.GetTrackInstructors(id);
            return View(instructors);
        }


        // Instructor confirmation attendance

        [HttpPost]
        public IActionResult ConfirmInstructorAttendance([FromRoute] int Id)
        {
            DateTime instructortDate = DateTime.Now;
            DateTime dateOnly = instructortDate.Date;
            string studentTime = instructortDate.ToString("hh:mm:ss");
            string correctTime = String.Format("09:00:00");

            Attendence instructorAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId = Id, UserType = UserTypeEnum.Student };


            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {

                instructorAttendance.IsPresent = true;
            }
            else if (comparison > 0)
            {
                instructorAttendance.IsLate = true;
            }
            else
            {
                instructorAttendance.IsPresent = true;
            }
            _attendance.ConfirmStudentAttendance(instructorAttendance);

            return RedirectToAction("GetInstructors");


        }


    }


    }



