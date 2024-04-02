namespace AttendenceSystem.Models
{
    public class Instructor:User
    {
        public DateOnly HireDate {  get; set; }
        public int Salary
        {
            get; set;
        }        
           

    }
}
