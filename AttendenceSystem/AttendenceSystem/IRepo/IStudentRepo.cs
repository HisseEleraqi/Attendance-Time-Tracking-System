using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {

        public Student StudentSchedule(int id);

        public Student GetStudentById(int userId);
    }

}
