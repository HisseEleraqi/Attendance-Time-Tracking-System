namespace AttendenceSystem.Models
{
    public enum EmployeeType
    {
        Security,
        StudentAffair

    }
    public class Employee:User
    {
        public EmployeeType EmpType { get; set; }
    }
}
