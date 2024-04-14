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


        // Foreign Key
        public int UserId {  get; set; }
        public virtual User User { get; set; }

        public int? TrackId { get; set; }
        public virtual Track Track { get; set; }
        public  UserTypeEnum UserType { get; set; }


    }
}
