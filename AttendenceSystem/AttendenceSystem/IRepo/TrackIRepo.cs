using AttendenceSystem.Data;
using AttendenceSystem.Models;

namespace AttendenceSystem.IRepo
{
    public interface TrackIRepo
    {
        public List<Track> GetAllTracks();
        public int NumberStudentRoledInTrack(int TrackId);
        public List<Student> GetTrackStudents(int TrackId);
        public List<Instructor> GetTrackInstructors(int TrackId);
        public Track GetTrackById(int TrackId);
        public int EditSupervisor(int TrackId, int SupervisorId);
        public int EditeActiveState(int TrackId, bool ActiveState);
        public List<Student> GetStudentsByTrackId(int trackId);

        List<Attendence> GetTodayAttendForTrackByDateAndTrackId(int TrackId, UserTypeEnum UserType);

    }
}
