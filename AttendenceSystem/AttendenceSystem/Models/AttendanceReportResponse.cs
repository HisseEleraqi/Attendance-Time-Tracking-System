namespace AttendenceSystem.Models
{
    public class AttendanceReportResponse
    {

        public  DateOnly InDate {  get; set; }

        public TimeOnly? InTime  { get; set; }
        public TimeOnly? OutTime { get; set; }

        public bool IsPresent { get; set; }

        public bool IsLate { get; set; }
        public bool IsAbsent { get; set; }
        public int Grade {  get; set; }
        
        public string userName { get; set; }

        public AttendanceEnum AttendanceStatue = AttendanceEnum.All;
    }
}
