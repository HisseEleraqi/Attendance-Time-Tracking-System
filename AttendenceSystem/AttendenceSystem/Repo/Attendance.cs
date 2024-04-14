using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

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
        public List<Attendence> GetAttendencesTrackId(int trackId, UserTypeEnum UserType)
        {

            var students = db.Attendences.AsNoTracking().Where(s => s.TrackId == trackId && s.UserType == UserType).ToList();
            return students;
        }
    }
}
