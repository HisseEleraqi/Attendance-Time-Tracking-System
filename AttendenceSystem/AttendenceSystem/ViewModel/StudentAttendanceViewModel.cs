
using AttendenceSystem.Models;

    namespace AttendenceSystem.ViewModel
    {
        public class StudentAttendanceViewModel
        {
            public int Id {  get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string University { get; set; }
            public string Faculty { get; set; }
            public int GraduationYear { get; set; }
            public int Degree { get; set; }
            public string TrackName { get; set; }

            public List<Attendence> Attendances { get; set; }

            public Permision Permission { get; set; }
        }
    }


