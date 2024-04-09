using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Instructor:User
    {
        [Required (ErrorMessage ="Please Enter Hire dAte")]
        public DateOnly HireDate {  get; set; }
        [Required(ErrorMessage = "Please Enter Slarary")]
        [Range(1000, 10000, ErrorMessage = "Salary must be between 1000 and 10000")]
        public int Salary {  get; set; }
        [Required(ErrorMessage = "Please Enter Tracks for Instructor")]
        [JsonIgnore]
        public virtual ICollection<InstructorTrack> TrackInstructors { get; set; } = new List<InstructorTrack>();
    
    }
}
