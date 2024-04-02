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



    }
}
