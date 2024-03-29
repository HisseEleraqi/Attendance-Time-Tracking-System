namespace AttendenceSystem.Models
{
    public class Schedule
    {
        public int Id {  get; set; }
        public DateOnly Date {  get; set; }

        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate {  get; set; }

        public TimeOnly Time {  get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
