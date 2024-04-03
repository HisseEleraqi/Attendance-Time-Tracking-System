using System.Reflection;

namespace AttendenceSystem.Models
{
    public enum gender
    {
        female,
        male
    }
    public class User
    {
        // primary key property
        public int Id { get; set; }

        // Properties for user details
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Img { get; set; }
        public gender Gender { get; set; }

        // Foreign key property for Role


        // Navigation property for Role
        public virtual List<Role> Roles { get; set; } = new List<Role>();
    }
}
