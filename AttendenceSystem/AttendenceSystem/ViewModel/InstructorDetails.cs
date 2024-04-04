namespace AttendenceSystem.ViewModel
{
    public class InstructorDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public DateOnly HireDate { get; set; }
        public int Salary { get; set; }

    }
}
