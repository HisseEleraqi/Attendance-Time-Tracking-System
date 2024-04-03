namespace AttendenceSystem.Models
{
    public class Permision
    {
        public int Id { get; set; }
        public string Type { get; set; }         
        public DateOnly Date { get; set; }

        public bool IsApproved { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

 
        // Foreign Key
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
