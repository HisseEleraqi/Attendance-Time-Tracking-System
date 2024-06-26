﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Role
    {
        public int Id { get; set; } 
        public string RoleName { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRole> Users { get; set; } = new List<UserRole>();


    }
}
