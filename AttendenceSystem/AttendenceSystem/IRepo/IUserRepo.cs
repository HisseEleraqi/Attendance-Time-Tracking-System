using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface IUserRepo
    {
        public List<User> GetUsersByRole(string roleName);
    }
}
