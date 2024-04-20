using AspNetCore.Reporting;
using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OfficeOpenXml;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //var students = _trackIRepo.GetStudentsByTrackId(id);
            ViewBag.ID = id;
            var trackAttendToday = _trackIRepo.GetTodayAttendForTrackByDateAndTrackId(id, UserTypeEnum.Student);
            ViewBag.TodayAttend= trackAttendToday;
            return View(trackAttendToday);
        }

        //[HttpPost("ExportToExcel/{ID}")]
        //public ActionResult ExportToExcel(int ID)
        //{
        //    try
        //    {

        //        //var Students = _trackIRepo.GetStudentsByTrackId(ID);
        //        var Students = _attendance.GetAttendencesTrackId(ID, UserTypeEnum.Student);

        //        List<string> Header = new List<string>();
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        using (ExcelPackage package = new ExcelPackage(new FileInfo("StudentReport.xlsx")))
        //        {

        //            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
        //            Header.Add("Id");

        //            Header.Add("Date");
        //            Header.Add("InTime");
        //            Header.Add("OutTime");
        //            Header.Add("Name");

        //            var headerRow = new List<string[]>()
        //            {
        //              Header.ToArray()
        //            };

        //            string headerRange = "A1:" + Char.ConvertFromUtf32(Header[0].Length + 64) + "1";
        //            worksheet.Cells[headerRange].LoadFromArrays(headerRow);

        //            int InsertRowIndex = 1;
        //            foreach (var item in Students)
        //            {
        //                InsertRowIndex++;
        //                worksheet.Cells[string.Format("A{0}", InsertRowIndex)].Value = item.Date;
        //                worksheet.Cells[string.Format("B{0}", InsertRowIndex)].Value = item.InTime;
        //                worksheet.Cells[string.Format("C{0}", InsertRowIndex)].Value = item.OutTime;
        //                worksheet.Cells[string.Format("D{0}", InsertRowIndex)].Value = item.User?.Name;

        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                package.SaveAs(stream);

        //                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StudentReport.xlsx");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //    }

        //}

        // Student confirmation attendance

        [HttpPost]
        public IActionResult ConfirmStudentAttendace([FromRoute] int Id, int id2)
        {
            var ShiftTime = context.Schedules.AsNoTracking().OrderByDescending(a=>a.Id).FirstOrDefault(a => a.TrackId == id2);
            DateTime date = DateTime.Now;
            DateTime currentdate = date.Date;
            string studentTime = date.ToString("hh:mm:ss");
            var correctTime = ShiftTime != null ? ShiftTime?.StartTime.Add(new TimeSpan(00, 15, 00)).ToTimeSpan() : new TimeSpan(00, 00, 00);// String.Format("09:00:00");

            //var attendance = _attendance.GetStudentAttendence(Id, currentdate);
            var attendance = _attendance.GetAttendence(Id, currentdate);
            if (attendance is null)
            {
                return StatusCode(404);

            }
            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = correctTime.Value;

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {
                attendance.IsPresent = true;
                attendance.IsAbsent = false;
            }
            else if (comparison > 0)
            {
                attendance.IsLate = true;
                attendance.IsAbsent = false;
            }
            else
            {
                attendance.IsPresent = true;
                attendance.IsAbsent = false;
            }

             attendance.InTime = TimeOnly.Parse(date.ToString("hh:mm:ss"));
             attendance.UserType = UserTypeEnum.Student;
             attendance.TrackId = id2;
             _attendance.SaveChanges();
            

            return RedirectToAction("GetAllTracks");


        }

        //[HttpPost]
        //public IActionResult ConfirmStudentAttendace([FromRoute]int Id,int id2)
        //{
        //    DateTime studentDate = DateTime.Now;
        //    DateTime dateOnly = studentDate.Date;
        //    string studentTime = studentDate.ToString("hh:mm:ss");
        //    string correctTime = String.Format("09:00:00");

        //    Attendence studentAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId =Id,UserType= UserTypeEnum.Student,TrackId=id2 };


        //    TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
        //    TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

        //    int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

        //    if (comparison == 0)
        //    {

        //        studentAttendance.IsPresent = true;
        //    }
        //    else if (comparison > 0)
        //    {
        //        studentAttendance.IsLate = true;
        //    }
        //    else
        //    {
        //        studentAttendance.IsPresent = true;
        //    }
        //   _attendance.ConfirmStudentAttendance(studentAttendance);

        //    return RedirectToAction("GetAllTracks");


        //}


        // Student Leaving Action

        [HttpPost]
        public IActionResult ConfirmStudentLeaving( [FromRoute] int id)
        {
            DateTime date = DateTime.Now;
            DateTime currentdate = date.Date;
            //var attendance = _attendance.GetStudentAttendence(id, currentdate);
            var attendance = _attendance.GetAttendence(id, currentdate);

            if (attendance != null)
            {
                attendance.OutTime = TimeOnly.Parse(DateTime.Now.ToString("hh:mm:ss"));
                _attendance.SaveChanges();
            }

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
            //var instructors = _trackIRepo.GetTrackInstructors(id);
            //ViewBag.ID = id;
            //var trackAttendToday = _trackIRepo.GetTodayAttendForTrackByDateAndTrackId(id);
            //ViewBag.TodayAttend = trackAttendToday;
            //return View(instructors);

            //var students = _trackIRepo.GetStudentsByTrackId(id);
            ViewBag.ID = id;
            var trackAttendToday = _trackIRepo.GetTodayAttendForTrackByDateAndTrackId(id, UserTypeEnum.Instructor);
            ViewBag.TodayAttend = trackAttendToday;
            return View(trackAttendToday);
        }


        // Instructor confirmation attendance


        [HttpPost("ConfirmInstructorAttendance/{Id}")]
        public IActionResult ConfirmInstructorAttendance([FromRoute] int Id, int id2)
        {

            var ShiftTime = context.Schedules.AsNoTracking().OrderByDescending(a => a.Id).FirstOrDefault(a => a.TrackId == id2);


            DateTime instructortDate = DateTime.Now;
            DateTime dateOnly = instructortDate.Date;
            string studentTime = instructortDate.ToString("hh:mm:ss");
            var a=new  TimeSpan(00, 15, 00);
            var correctTime = ShiftTime!=null? ShiftTime?.StartTime.Add(new TimeSpan(00, 15, 00)).ToTimeSpan() : new TimeSpan(00, 00, 00);//String.Format("12:0:00");

            //Attendence instructorAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId = Id, UserType = UserTypeEnum.Instructor, TrackId = id2 };
            //if ()
            //{

            //}
            //var attendance = _attendance.GetStudentAttendence(Id, dateOnly);
            var attendance = _attendance.GetAttendence(Id, dateOnly);

            if (attendance is null)
            {
                return StatusCode(404);

            }

            TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
            TimeSpan correctTimeSpan = correctTime.Value ; // TimeSpan.Parse(correctTime);

            int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

            if (comparison == 0)
            {

                attendance.IsPresent = true;
                attendance.IsAbsent = false;

            }
            else if (comparison > 0)
            {
                attendance.IsLate = true;
                attendance.IsAbsent = false;

            }
            else
            {
                attendance.IsPresent = true;
                attendance.IsAbsent = false;
            }
            attendance.InTime = TimeOnly.Parse(dateOnly.ToString("hh:mm:ss"));
            attendance.UserType = UserTypeEnum.Instructor;
            attendance.TrackId = id2;
            _attendance.SaveChanges();

           // _attendance.ConfirmStudentAttendance(instructorAttendance);

            return RedirectToAction("GetAllInstructorsTracks");


        }

        //[HttpPost("ConfirmInstructorAttendance/{Id}")]
        //public IActionResult ConfirmInstructorAttendance([FromRoute] int Id, int id2)
        //{
        //    DateTime instructortDate = DateTime.Now;
        //    DateTime dateOnly = instructortDate.Date;
        //    string studentTime = instructortDate.ToString("hh:mm:ss");
        //    string correctTime = String.Format("12:0:00");

        //    Attendence instructorAttendance = new Attendence() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), InTime = TimeOnly.Parse(studentTime), UserId = Id, UserType = UserTypeEnum.Instructor, TrackId = id2 };
        //    //if ()
        //    //{

        //    //}

        //    TimeSpan studentTimeSpan = TimeSpan.Parse(studentTime);
        //    TimeSpan correctTimeSpan = TimeSpan.Parse(correctTime);

        //    int comparison = TimeSpan.Compare(studentTimeSpan, correctTimeSpan);

        //    if (comparison == 0)
        //    {

        //        instructorAttendance.IsPresent = true;
        //    }
        //    else if (comparison > 0)
        //    {
        //        instructorAttendance.IsLate = true;
        //    }
        //    else
        //    {
        //        instructorAttendance.IsPresent = true;
        //    }
        //    _attendance.ConfirmStudentAttendance(instructorAttendance);

        //     return RedirectToAction("GetAllInstructorsTracks");


        //}

        // Instructor Leaving

        [HttpPost("ConfirmInstructorLeaving/{Id}")]
        public IActionResult ConfirmInstructorLeaving(int Id)
        {
            DateTime date = DateTime.Now;
            DateTime currentdate = date.Date;
            //var attendance = _attendance.GetStudentAttendence(Id, currentdate);
            var attendance = _attendance.GetAttendence(Id, currentdate);

            if (attendance != null)
            {
                attendance.OutTime = TimeOnly.Parse(DateTime.Now.ToString("hh:mm:ss"));
                _attendance.SaveChanges();
            }

            return RedirectToAction("GetAllInstructorsTracks");


        }

    }


    

}




