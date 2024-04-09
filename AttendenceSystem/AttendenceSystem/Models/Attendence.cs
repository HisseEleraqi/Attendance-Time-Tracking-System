using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Attendence
    {
        public int Id {  get; set; }
        public DateOnly Date {  get; set; }

        public TimeOnly InTime {  get; set; }
        public TimeOnly OutTime {  get; set; }
        public bool IsPresent {  get; set; }
        public bool IsAbsent {  get; set; }


        public bool IsLate {  get; set; }

        [JsonIgnore]
        // Foreign Key
        public int UserId {  get; set; }
        public virtual User User { get; set; }


    }
}
