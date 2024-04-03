using AttendenceSystem.Data;
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace AttendenceSystem.Repo
{
    public interface IAccountRepo
    {
        public User GetUser(LoginViewModel model);


    }
    public class AccountRepo : IAccountRepo
    {
        DataContext db = new DataContext();
        public User GetUser(LoginViewModel model)
        {
            var user = db.Users.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }


}



