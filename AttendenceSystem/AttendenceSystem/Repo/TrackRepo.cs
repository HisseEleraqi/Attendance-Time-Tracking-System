using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

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

        public List<Track> GetActiveTracks()
        {
            return context.Tracks.Where(t => t.IsActive).ToList();

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
        //public List<StudentAttendanceViewModel> GetTrackAttendance(int trackId)
        //{
        //    // Retrieve all students in the given track
        //    List<Student> students = GetTrackStudents(trackId);

        //    // Create a list to store attendance view models
        //    List<StudentAttendanceViewModel> viewModels = new List<StudentAttendanceViewModel>();

        //    // Iterate through each student to retrieve their attendance information
        //    foreach (var student in students)
        //    {
        //        // Retrieve the attendance information for the current student
        //        StudentAttendanceViewModel viewModel = studentService.GetStudentAttendance(student.Id);

        //        // Add the attendance view model to the list
        //        viewModels.Add(viewModel);
        //    }

        //    return viewModels;
        //}


    }

}
