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

       

        public Student StudentSchedule(int id)
        {
           
            var StudentSchedule =context.Students.Include(d=>d.Track.Schedules).FirstOrDefault(s => s.Id == id);

            return StudentSchedule;
      

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


        

        



    }
}
