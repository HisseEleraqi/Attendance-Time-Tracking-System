using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {

        public List<Student> GetAllStudents();
        public void UpdateStudent(Student student);
        public void AddStudent(Student student);
        public Student GetStudentById(int userId);
        public List<Schedule> StudentSchedule(int studentId);
        public int GetStudentLateDays(int studentId);
        public int GetStudentAbsentDays(int studentId);
        public int GetStudentDegrees(int StudentId);
        public List<Permision> GetStudentPermision(int StudentId);
        public void Addnewpermision(Permision newpermision);
        public void Deletpermision(int permisionId);
        public List<Attendence> GetAttendancesByStudentId(int studentId);
        public Permision GetPermissionByStudentId(int studentId);
        public List<Attendence> GetStudentAttendances(int studentId, DateOnly date);
        public StudentAttendanceViewModel UpdateStudentDegree(int id, int perid);
        public void DeleteStudent(int studentid);
        public List<Track> GetTracks();
        public Track GetStudentTrack(int studentid);
        public void EditStudent(Student editedstudent);
        public Task<List<Student>> GetPendingStudentsAsync();
        public void UpdateStudentState(int Id);
        public int AllActiveTracks();
        public int AllInActiveTracks();
        public int AllAccepptedStudent();
        public int Allinstructor();
        public int AllSupervisor();
        public List<Student> GetAllAcceptedStudents();
    }

}
