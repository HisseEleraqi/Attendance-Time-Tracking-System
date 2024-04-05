using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {

        public Student StudentSchedule(int id);

        public Student GetStudentById(int userId);

        public int GetStudentLateDays(int id);

        public int GetStudentAbsentDays(int id);


        public int GetStudentDegrees(int id);

        


    }

}
