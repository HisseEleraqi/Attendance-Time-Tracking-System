﻿using System.Text.Json.Serialization;

namespace AttendenceSystem.Models
{
    public class Student:User
    {
       public int Degree {  get; set; }
       public string Specification { get; set; }
       public int GraduationYear {  get; set; }
       public string Faculty { get; set; }
       public string University {  get; set; }
       public bool IsAccepted { get; set; }
       public int TrackID {  get; set; }
        [JsonIgnore]
        public virtual  Track Track { get; set; }
        [JsonIgnore]
        public virtual ICollection<Permision> Permisions { get; set; } = new List<Permision>();

    }
}
