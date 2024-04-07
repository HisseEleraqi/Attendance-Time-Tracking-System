using System;
using System.ComponentModel.DataAnnotations;

namespace AttendenceSystem.Models
{
    public enum PermisionType
    {
        Absence,
        Late
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

        [Required(ErrorMessage = "Permission type is required.")]
        public PermisionType Type { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "Reason  is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Reason must be between 5 and 100 characters.")]
        public string Reason { get; set; }
        public PermisionStatus Status { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}
