using System.ComponentModel.DataAnnotations;

namespace AttendenceSystem.Models
{
   
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[^A-Za-z0-9]).{8,}$")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a mobile number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number")]
        public string Mobile { get; set; }

        
        public virtual ICollection<UserRole> Roles { get; set; }=new List<UserRole>();

    }
}
