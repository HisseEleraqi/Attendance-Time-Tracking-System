using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;

namespace AttendenceSystem.Repo
{

    public class Attendance : IAttendance
    {

        private readonly DataContext db = new DataContext();
        public void ConfirmStudentAttendance(Attendence studentAttendance)
        {
         
            db.Attendences.Add(studentAttendance);
            db.SaveChanges();
        }
    }
}
