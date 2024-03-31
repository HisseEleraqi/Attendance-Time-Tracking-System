namespace AttendenceSystem.Models
{
    public class User
    {
        // primary key property
        public int Id { get; set; }

        // Properties for user details
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }

        // Foreign key property for Role
        public int RoleId { get; set; }

        // Navigation property for Role
        public virtual Role Role { get; set; }
    }
}
