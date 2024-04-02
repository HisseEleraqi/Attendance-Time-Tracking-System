using AttendenceSystem.Models;
using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public int GetLateDays(int InstructorId)
        {
            var result=context.Attendences.Where(user=>user.UserId== InstructorId && user.IsLate==true).Count();
            return result;
        }
        public int GetAbsentDays(int InstructorId)
        {
            var result = context.Attendences.Where(user => user.UserId == InstructorId && user.IsAbsent==false).Count();
            return result;
        }
        public List<Track> GetTracksExceptInstructorTrack(int instructorId)
        {
            var instructor = context.Instructors.FirstOrDefault(i => i.Id == instructorId);
            if (instructor == null || instructor.TrackInstructors == null || !instructor.TrackInstructors.Any())
            {
                return context.Tracks.ToList();
            }
            var allTracks = context.Tracks.ToList();
            var instructorTrackIds = instructor.TrackInstructors.Select(ti => ti.TrackId).ToList();
            var tracksExceptInstructor = allTracks.Where(t => !instructorTrackIds.Contains(t.Id)).ToList();
            return tracksExceptInstructor;
        }
        public void EditInstructor(int id, InstructorTrackViewModel instructor)
        {
            var existingInstructor = context.Instructors.FirstOrDefault(i => i.Id == id);

            if (existingInstructor != null)
            {
                existingInstructor.Name = instructor.Name;
                existingInstructor.Email = instructor.Email;
                existingInstructor.Mobile = instructor.Mobile;
                existingInstructor.Salary = instructor.Salary;
                existingInstructor.HireDate = instructor.HireDate;
                existingInstructor.Password = instructor.Password;

                UpdateInstructorTracks(existingInstructor, instructor.Tracks);
                context.SaveChanges();
            }
        }

        private void UpdateInstructorTracks(Instructor instructor, List<int> trackIds)
        {
            foreach (var track in instructor.TrackInstructors.ToList())
            {
                if (!trackIds.Contains(track.TrackId))
                {
                    instructor.TrackInstructors.Remove(track);
                    context.instructorTracks.Remove(track);
                }
            }
            foreach (var trackId in trackIds)
            {
                if (!instructor.TrackInstructors.Any(t => t.TrackId == trackId))
                {
                    var newInstructorTrack = new InstructorTrack
                    {
                        InstructorId = instructor.Id,
                        TrackId = trackId
                    };
                    context.instructorTracks.Add(newInstructorTrack);
                }
            }
        }

        public bool ISEmailExistE(int id,string email)
        {
            var result = context.Users.FirstOrDefault(u => u.Email == email && u.Id!=id);
            if (result == null)
            {
                return false;
            }
            else
                return true;
        }

        public void DeleteInstructor(int instructorid)
        {
            var instructor = GetInstructor(instructorid);
            context.Instructors.Remove(instructor);
            context.SaveChanges();
        }
    }
}
