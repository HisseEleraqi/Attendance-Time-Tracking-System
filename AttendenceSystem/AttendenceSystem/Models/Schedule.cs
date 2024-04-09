using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Schedule
    {
        public int Id {  get; set; }
        public DateOnly Date {  get; set; }

        public TimeOnly StartTime {  get; set; }
        public TimeOnly EndTime {  get; set; }
        public int TrackId {  get; set; }
        [JsonIgnore]
        public virtual Track Tracks { get; set; }
    }
}
