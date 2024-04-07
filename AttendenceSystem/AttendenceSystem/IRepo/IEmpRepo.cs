using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IEmpRepo
    {
        public List<Employee> GetAllEmployees();
        public Employee GetEmployeeById(int id);
        public void AddEmployee(Employee employee);
        public void UpdateEmployee(Employee employee);
        public void DeleteEmployee(int id);
    }

}
