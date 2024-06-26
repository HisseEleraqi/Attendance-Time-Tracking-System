﻿using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{
    public class UserRepo:IUserRepo
    {
        private readonly DataContext db = new DataContext();

        public List<User> GetUsersByRole(string roleName)
        {
            return db.Set<User>()
                      .Where(u => u.Roles.Any(r => r.Role.RoleName == roleName))
                      .ToList();
        }
        public List<User> GetStudentsWithAttendance(string roleName)
        {
            var x = db.Users
                    .Include(u => u.Attendances) // Include attendance records for eager loading
                    .Where(u => u.Roles.Any(r => r.Role.RoleName == roleName) && u.Attendances.Any())
                    .ToList();
            Console.WriteLine(x);
            
            return x;
        }
      

    }
}
