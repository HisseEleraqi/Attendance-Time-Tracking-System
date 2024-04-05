namespace AttendenceSystem.IRepo
{
    public interface StudentIRepo
    {

        public int StudentDetails(int id);
        public void StudentScdule(int id);
        public int GetStudentLateDays(int StudentId);
        public int GetStudentAbsentDays(int StudentId);
        public int GetStudentDegrees(int StudentId);



    }
}
