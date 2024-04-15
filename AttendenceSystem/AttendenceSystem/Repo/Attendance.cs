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

        public Attendence GetStudentAttendence(int studentId, DateTime date)
        {
            var studentAttendence = db.Attendences.Where(a => a.UserId == studentId && a.Date == DateOnly.Parse(date.ToString("yyyy-MM-dd"))).FirstOrDefault();

            return studentAttendence;

        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
