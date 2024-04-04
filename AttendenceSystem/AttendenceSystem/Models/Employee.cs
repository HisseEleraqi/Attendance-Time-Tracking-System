namespace AttendenceSystem.Models
{
    public enum EmployeeType
    {
        Security,
        Student_affairs

    }
    public class Employee:User
    {
        public EmployeeType EmpType { get; set; }
       
    }
}
