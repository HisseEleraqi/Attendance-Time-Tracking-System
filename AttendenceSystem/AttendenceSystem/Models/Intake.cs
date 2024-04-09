using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Intake
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    }

}
