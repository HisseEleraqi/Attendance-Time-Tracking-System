using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class InstructorTrack
    {
            public int TrackId { get; set; }
        [JsonIgnore]
        public virtual Track Track { get; set; }

            public int InstructorId { get; set; }
        [JsonIgnore]
        public virtual Instructor Instructor { get; set; }
      
    }
}
