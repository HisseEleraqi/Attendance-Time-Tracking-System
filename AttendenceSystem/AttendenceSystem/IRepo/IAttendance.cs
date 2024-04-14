using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IAttendance
    {


        public void ConfirmStudentAttendance(Attendence studentAttendance);
        public List<Attendence> GetAttendencesTrackId(int trackId, UserTypeEnum UserType);

    }
}
