namespace AttendenceSystem.Models
{
    public class Instructor:User
    {
        public DateOnly HireDate {  get; set; }
<<<<<<< HEAD
        public int Salary
        {
            get; set;
        }        
           
=======
        public int Salary {  get; set; }
        public virtual ICollection<InstructorTrack> TrackInstructors { get; set; } = new List<InstructorTrack>();
>>>>>>> fc018798f8f887803e3fde870d3f306087700652

    }
}
