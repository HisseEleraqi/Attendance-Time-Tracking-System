using AttendenceSystem.Data;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{
    public class StudentRepo
    {
        private readonly DataContext context = new DataContext();
       

        public int StudentDetails(int id)
        {

            return (context.Students.FirstOrDefault(s => s.Id == id).Id);

        }

        public void StudentScdule(int id)
        {
           
            var StudentSchedule =context.Students.Include(d=>d.Track.Schedules).FirstOrDefault(s => s.Id == id);

        }



    }
}
