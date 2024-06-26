﻿using AttendenceSystem.Data;
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;

namespace AttendenceSystem.IRepo
{
    public interface TrackIRepo
    {
        public List<Track> GetAllTracks();

        public List<Track> GetActiveTracks();


        public int NumberStudentRoledInTrack(int TrackId);
        public List<Student> GetTrackStudents(int TrackId);
        public List<Instructor> GetTrackInstructors(int TrackId);
        public Track GetTrackById(int TrackId);
        public int EditSupervisor(int TrackId, int SupervisorId);
        public int EditeActiveState(int TrackId, bool ActiveState);

        //public List<StudentAttendanceViewModel> GetTrackAttendance(int trackId);




    }
}
