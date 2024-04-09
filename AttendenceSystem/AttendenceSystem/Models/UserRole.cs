using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class UserRole
    {
   
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
