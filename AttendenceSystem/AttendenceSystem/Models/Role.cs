using System.ComponentModel.DataAnnotations;

namespace AttendenceSystem.Models
{
    public class Role
    {
        public int Id { get; set; } 
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }=new List<User>();
    }
}
