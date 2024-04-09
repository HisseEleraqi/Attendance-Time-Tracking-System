using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Programs
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
