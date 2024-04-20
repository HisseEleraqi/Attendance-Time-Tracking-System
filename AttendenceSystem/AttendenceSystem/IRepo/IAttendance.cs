using AttendenceSystem.Models;
using AttendenceSystem.Repo;

namespace AttendenceSystem.IRepo
{
    public interface IAttendance
    {


        public void ConfirmStudentAttendance(Attendence studentAttendance);
        public List<Attendence> GetAttendencesTrackId(int trackId, UserTypeEnum UserType);
        public Attendence GetStudentAttendence(int studentId, DateTime date);
        public Attendence GetAttendence(int Id, DateTime date);

        // filtering with date and attendance state
        List<Attendence> GetAbsentStudents(DateOnly startDate, DateOnly endDate);
        List<Attendence> GetLateStudents(DateOnly startDate, DateOnly endDate);
        List<Attendence> GetPresentStudents(DateOnly startDate, DateOnly endDate);

        public void SaveChanges();


    }
}
