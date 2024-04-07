using System.ComponentModel.DataAnnotations;
using AttendenceSystem.Models;
namespace AttendenceSystem.ViewModel
{
    public class InstructorTrackViewModel
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
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        [Required(ErrorMessage = "Please Enter Hire dAte")]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Please Enter Slarary")]
        [Range(1000, 10000, ErrorMessage = "Salary must be between 1000 and 10000")]
        public int Salary { get; set; }
        [Required(ErrorMessage = "Please Enter Tracks for Instructor")]
        public virtual List<int> Tracks { get; set; } = new List<int>();
    }
}
    