using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;

namespace AttendenceSystem.IRepo
{
    public interface InstructorIRepo 
    {
        public List<Instructor> GetAllInstructors();
        public void AddInstructor(InstructorTrackViewModel instructor);
        public List<Track> GetAllTracks();
        public List<Track> GetInstructorTrack(int InstructorId);
        public Instructor GetInstructor(int InstructorId);
        public bool ISEmailExist(string email);
        public int GetLateDays(int InstructorId);
        public int GetAbsentDays(int InstructorId);
        public List<Track> GetTracksExceptInstructorTrack(int instructorId);
        public bool ISEmailExistE(int id, string email);
        public void EditInstructor(int id, InstructorTrackViewModel instructor);
        public void DeleteInstructor(int instructorid);
        public int GetRole();


    }
}
