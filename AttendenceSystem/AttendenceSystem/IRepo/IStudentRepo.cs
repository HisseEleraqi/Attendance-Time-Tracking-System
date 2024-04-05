using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {
        public Student StudentDetails(int id);

        public Student StudentSchedule(int id);

        public Student GetStudentById(int userId);
        public int Id { get; set; }
    }
}
