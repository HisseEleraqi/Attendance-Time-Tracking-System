using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IStudentRepo
    {

       
        public void AddStudent(Student student);

  
        public Schedule StudentSchedule(int id);


        public Student GetStudentById(int userId);

        public int GetStudentLateDays(int id);

        public int GetStudentAbsentDays(int id);


        public int GetStudentDegrees(int id);
        public List<Permision> GetStudentPermision(int StudentId);

        public void Addnewpermision(Permision newpermision);


        public void Deletpermision(int permisionId);

    }

}
