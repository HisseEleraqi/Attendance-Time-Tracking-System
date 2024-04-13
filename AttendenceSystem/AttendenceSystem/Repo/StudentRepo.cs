using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{

    public class StudentRepo:IStudentRepo

    {
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





    }
}
