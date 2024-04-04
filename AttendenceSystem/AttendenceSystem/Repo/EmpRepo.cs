using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{
    public class EmpRepo:IEmpRepo
    {
        private readonly DataContext db=new DataContext();
        
        public List<Employee> GetAllEmployees()
        {
            return db.Employees.ToList();
        }
        public Employee GetEmployeeById(int id)
        {
            return db.Employees.Find(id);
        }
       
        public void AddEmployee(Employee employee)
        {
          var EmpRole=db.Roles.FirstOrDefault(r => r.RoleName == "Employee");
            
            var emp = new Employee
            {
                Name = employee.Name,
                Email = employee.Email,
                Mobile = employee.Mobile,
                Password = employee.Password,
                EmpType = employee.EmpType,
                
            };
            db.Employees.Add(emp);
            db.SaveChanges();
            var empId =emp.Id;
            db.UserRoles.Add(new UserRole { UserId = empId, RoleId = EmpRole.Id });
            db.SaveChanges();

        }
        public void UpdateEmployee(Employee employee)
        {
            var emp = db.Employees.Find(employee.Id);
            emp.Name = employee.Name;
            emp.Email = employee.Email;
            emp.Mobile = employee.Mobile;
            emp.Password = employee.Password;
            emp.EmpType = employee.EmpType;
            db.Employees.Update(emp);             
            db.SaveChanges();
        }
        public void DeleteEmployee(int id)
        {
            var employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
        }



    }
}
