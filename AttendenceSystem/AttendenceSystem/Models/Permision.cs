namespace AttendenceSystem.Models
{
  public  enum PermisionType
    {
        Absence,
        Late,
        EarlyLeave
    }
    public enum PermisionStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class Permision
    {
        public int Id { get; set; }
        public PermisionType Type { get; set; }    
        
        public DateOnly Date { get; set; }

        public bool IsApproved { get; set; }

        public string Reason { get; set; }

        public PermisionStatus Status { get; set; }

 
        // Foreign Key
        public Student StudentId { get; set; }
    }
}
