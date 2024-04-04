using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using System.Diagnostics.Contracts;

namespace AttendenceSystem.Repo
{
    public class TrackRepo: TrackIRepo
    {
        private readonly DataContext context = new DataContext();
        public List<Track> GetAllTracks()
        {
           return context.Tracks.ToList();

        }
        public Track GetTrackById(int TrackId)
        {
            return context.Tracks.FirstOrDefault(t=>t.Id == TrackId);
        }
        public List<Student> GetTrackStudents(int TrackId)
        {
            return context.Students.Where(s => s.TrackID == TrackId && s.IsAccepted == true).ToList();
        }
        public List<Instructor> GetTrackInstructors(int TrackId)
        {
            var intructorId = context.instructorTracks.Where(instructortrack => instructortrack.TrackId == TrackId).ToList();
            List<Instructor> instructors = new List<Instructor>();
            foreach (var instructor in intructorId)
            {
                instructors.Add(context.Instructors.FirstOrDefault(i => i.Id == instructor.InstructorId));
            }
            return instructors;

        }
        public int NumberStudentRoledInTrack(int TrackId)
        {
            var result=context.Students.Count(s=>s.TrackID== TrackId && s.IsAccepted==true);
            return result;
        }
        public int EditSupervisor(int TrackId,int SupervisorId)
        {
            try
            {
                var track = context.Tracks.FirstOrDefault(t => t.Id == TrackId);
                track.SupervisorId = SupervisorId;
                context.SaveChanges();
                return 1;
            }
           catch
            {
                return 0;
            }
        }
        public int EditeActiveState(int TrackId, bool ActiveState)
        {
            try
            {
                var track = context.Tracks.FirstOrDefault(t => t.Id == TrackId);
                track.IsActive = ActiveState;
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

    }

}
