
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;

public interface IStudentService
{
    public StudentAttendanceViewModel GetStudentAttendance(int studentId);
    public List<StudentAttendanceViewModel> GetTrackAttendancedate(int trackId , DateOnly date);
    public List<StudentAttendanceViewModel> GetStudentAttendancedate(int studentId, DateOnly date);

}



