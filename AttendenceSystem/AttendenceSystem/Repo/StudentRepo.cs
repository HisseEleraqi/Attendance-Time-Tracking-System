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

        public Student StudentDetails(int id)
        {

            return context.Students.FirstOrDefault(s => s.Id == id);

        }

        public Student StudentSchedule(int id)
        {
           
            var StudentSchedule =context.Students.Include(d=>d.Track.Schedules).FirstOrDefault(s => s.Id == id);

            return StudentSchedule;
      

        }

        ///how to get the student id from the session 
        ///

        public int Id { get; set; }
        



    }
}
