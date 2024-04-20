using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{

    public class AttendanceRepo : IAttendance
    {

        private readonly DataContext db = new DataContext();
        public void ConfirmStudentAttendance(Attendence studentAttendance)
        {
            db.Attendences.Add(studentAttendance);
            db.SaveChanges();
        }
        public List<Attendence> GetAttendencesTrackId(int trackId, UserTypeEnum UserType)
        {

            var students = db.Attendences.Include(a=>a.User).AsNoTracking().Where(s => s.TrackId == trackId && s.UserType == UserType).ToList();
            return students;
        }

        public Attendence GetStudentAttendence(int studentId, DateTime date)
        {
            var studentAttendence = db.Attendences.FirstOrDefault(a => a.UserId == studentId && a.Date == DateOnly.Parse(date.ToString("yyyy-MM-dd")));

            return studentAttendence;

        }
        public Attendence GetAttendence(int Id, DateTime date)
        {
            var studentAttendence = db.Attendences.FirstOrDefault(a => a.Id == Id && a.Date == DateOnly.Parse(date.ToString("yyyy-MM-dd")));

            return studentAttendence;

        }


        // filtering with date and attendance state
        public List<Attendence> GetAbsentStudents(DateOnly startDate, DateOnly endDate)
        {
            return db.Attendences.Where(a => a.IsAbsent == true && a.Date >= startDate && a.Date <= endDate).ToList();

        }

        public List<Attendence> GetLateStudents(DateOnly startDate, DateOnly endDate)
        {
            return db.Attendences.Where(a => a.IsLate == true && a.Date >= startDate && a.Date <= endDate).ToList();
        }

        public List<Attendence> GetPresentStudents(DateOnly startDate, DateOnly endDate)
        {
            return db.Attendences.Where(a => a.IsPresent == true && a.Date >= startDate && a.Date <= endDate).ToList();
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }


        
    }
}
