namespace AttendenceSystem.Models
{
    public class Schedule
    {
        public int Id {  get; set; }
        public DateOnly Date {  get; set; }

        public TimeOnly StartTime {  get; set; }
        public TimeOnly EndTime {  get; set; }

       /* public TimeOnly Time {  get; set; }*/
        public int TrackId {  get; set; }
        public virtual Track Tracks { get; set; }

    }
}
