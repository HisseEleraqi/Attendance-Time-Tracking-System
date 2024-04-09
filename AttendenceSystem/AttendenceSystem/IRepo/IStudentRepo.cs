using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {

        public List<Student> GetAllStudents();
        public void AddStudent(Student student);


  
        public Schedule StudentSchedule(int id);


        public Student GetStudentById(int userId);
        public StudentAttendanceViewModel UpdateStudentDegree(int id, int newDegree);

        public int GetStudentLateDays(int id);

        public int GetStudentAbsentDays(int id);


        public int GetStudentDegrees(int id);
        public List<Permision> GetStudentPermision(int StudentId);

        public void Addnewpermision(Permision newpermision);


        public void Deletpermision(int permisionId);
        List<Attendence> GetAttendancesByStudentId(int studentId);
        Permision GetPermissionByStudentId(int studentId);
        public List<Attendence> GetStudentAttendances(int studentId, DateOnly date);

    }

}
