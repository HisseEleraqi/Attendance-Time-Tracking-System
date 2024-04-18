using AspNetCore.Reporting;
using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace AttendenceSystem.Repo
{

    public class StudentRepo:IStudentRepo

    {
        private readonly IReportService _reportService;
        public StudentRepo()
        {
        }
        public StudentRepo(IReportService reportService)
        {
            _reportService = reportService;
        }
        private readonly DataContext context = new DataContext();


          public Student GetStudentById(int userId)
        {

            return context.Students.FirstOrDefault(s => s.Id == userId);

        }



        public Schedule StudentSchedule(int studentId)
        {
            var schedule = context.Students
                                  .Where(s => s.Id == studentId)
                                  .SelectMany(s => s.Track.Schedules)
                                 
                                  .OrderByDescending(sch => sch.Date)
                                  .FirstOrDefault(); 

            return schedule;
        }
        public int GetStudentLateDays(int studentId)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today); 
            return context.Attendences.Count(s => s.IsLate && s.UserId == studentId && s.Date <= startDate);
        }

        public int GetStudentAbsentDays(int studentId)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today); 
            return context.Attendences.Count(s => s.IsAbsent && s.UserId == studentId && s.Date <= startDate);
        }
        public int GetStudentDegrees(int StudentId)
        {
            var degree = context.Students.FirstOrDefault(s => s.Id == StudentId).Degree;
            var LateMinus = GetStudentLateDays(StudentId)*5;
            var AbsentMinus=GetStudentAbsentDays(StudentId)*10;
            return degree - (LateMinus + AbsentMinus);
        }
        public List<Permision>GetStudentPermision(int StudentId)
        {
           return context.Permisions.Where(p=>p.StudentId==StudentId).ToList();

        }
        public void Addnewpermision(Permision newpermision)
        {
            context.Permisions.Add(newpermision);
            context.SaveChanges();

        }

        public void Deletpermision(int permisionId)
        {
            var permision=context.Permisions.FirstOrDefault(p => p.Id == permisionId);
            context.Permisions.Remove(permision);
            context.SaveChanges();
        }


        public void AddUser(User user)
        {
            context.Users.Add(user);    
            context.SaveChanges();  
        }

        public void AddStudent(Student user)
        {
            context.Students.Add(user);
            context.SaveChanges();
        }

        public async Task<byte[]> PrintStudentReport(RenderType rendertype, int TrackId)
        {
            var students =  context.Students.Where(s => s.TrackID == TrackId).ToList();
            var report = await _reportService.DownloadReport(students.ToList(), "StudentReport", rendertype);
            return report.MainStream;
        }

        public void GetAllUsers()
        {
            DateTime studentDate = DateTime.Now;
            DateTime dateOnly = studentDate.Date;
            List<Attendence> AttendenceList = new();
            var Users = context.Users.Select(a => a.Id).ToList();

            foreach (var Id in Users)
            {
                Attendence Attendance = new() { Date = DateOnly.Parse(dateOnly.ToString("yyyy-MM-dd")), UserId = Id, IsAbsent = true };
                AttendenceList.Add(Attendance);

            }

            context.Attendences.AddRange(AttendenceList);
            context.SaveChanges();
        }
    }
}
