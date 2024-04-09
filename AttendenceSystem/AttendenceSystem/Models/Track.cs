using Newtonsoft.Json;

namespace AttendenceSystem.Models
{
    public class Track
    { 
        public int Id {  get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }

        public int ProgramId {  get; set; }
        [JsonIgnore]
        public virtual Programs Program { get; set; }
        public int SupervisorId {  get; set; }
        [JsonIgnore]
        public virtual Instructor Supervisor { get; set; }
        [JsonIgnore]
        public virtual ICollection<Intake> Intake { get; set; } = new List<Intake>();
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        [JsonIgnore]
        public virtual ICollection<InstructorTrack> Instructors { get; set; } = new List<InstructorTrack>();
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
