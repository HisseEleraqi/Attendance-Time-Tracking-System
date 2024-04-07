namespace AttendenceSystem.Models
{
    public class InstructorTrack
    {
            public int TrackId { get; set; }
            public virtual Track Track { get; set; }

            public int InstructorId { get; set; }
            public virtual Instructor Instructor { get; set; }
      
    }
}
