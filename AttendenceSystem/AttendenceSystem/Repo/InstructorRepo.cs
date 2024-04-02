using AttendenceSystem.Models;
using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.ViewModel;
namespace AttendenceSystem.Repo
{
    public class InstructorRepo: InstructorIRepo
    {
        private readonly DataContext context=new DataContext();

        public List<Instructor> GetAllInstructors()
        {
           var instructors= context.Instructors.ToList();
            return instructors;
        }
        public void AddInstructor(InstructorTrackViewModel instructor)
        {
            Instructor newInstructor = new Instructor
            {
                Name = instructor.Name,
                Email = instructor.Email,
                Mobile = instructor.Mobile,
                Salary = instructor.Salary,
                HireDate = instructor.HireDate,
                Password = instructor.Password,
                RoleId = context.Roles.FirstOrDefault(r => r.RoleName == "Instructor").Id
            };
            context.Instructors.Add(newInstructor);
            context.SaveChanges();
            int instructorId = newInstructor.Id;
            foreach (var trackId in instructor.Tracks)
            {
                InstructorTrack instructorTrack = new InstructorTrack
                {
                    InstructorId = instructorId,
                    TrackId = trackId
                };
                context.instructorTracks.Add(instructorTrack);
            }
            context.SaveChanges();
        }
        public List<Track> GetAllTracks()
        {
            var Tracks = context.Tracks.ToList();
            return Tracks;
        }
        public List<Track> GetInstructorTrack(int InstructorId)
        {
            var instructor= context.Instructors.FirstOrDefault(I=>I.Id==InstructorId);
            var tracks= instructor.TrackInstructors.ToList();
            List<Track> result = new List<Track>();
            foreach (var item in tracks)
            {
                var track = context.Tracks.FirstOrDefault(t => t.Id == item.TrackId);
                result.Add(track);
                
            }
            return result;
        }
        public Instructor GetInstructor(int InstructorId)
        {
           return  context.Instructors.FirstOrDefault(i => i.Id == InstructorId);

        }
        public bool ISEmailExist(string email)
        {
            var result=context.Users.FirstOrDefault(u => u.Email == email);
            if (result == null)
            {
                return false;
            }
            else
                return true;
        }
    }
}
