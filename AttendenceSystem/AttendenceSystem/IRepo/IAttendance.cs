using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IAttendance
    {


        public void ConfirmStudentAttendance(Attendence studentAttendance);
        
    }
}
