using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;
using System;
using System.Collections.Generic;

namespace AttendenceSystem.Repo
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo studentRepo;
        private readonly TrackIRepo trackIRepo;

        public StudentService(IStudentRepo _studentRepo, TrackIRepo _trackIRepo)
        {
            studentRepo = _studentRepo;
            trackIRepo = _trackIRepo;
        }

        public StudentAttendanceViewModel GetStudentAttendance(int studentId)
        {
            var student = studentRepo.GetStudentById(studentId);
            if (student == null)
            {
                // Handle the case when student is not found
                return null;
            }

            var attendances = studentRepo.GetAttendancesByStudentId(studentId);
            var permission = studentRepo.GetPermissionByStudentId(studentId);

            // Map data to view model
            StudentAttendanceViewModel viewModel = new StudentAttendanceViewModel
            {
                Id=student.Id,
                Name = student.Name,
                Email = student.Email,
                Mobile = student.Mobile,
                Degree = student.Degree,
                University = student.University,
                Faculty = student.Faculty,
                GraduationYear = student.GraduationYear,
                TrackName = student.Track?.Name, // Access track name if it's available
                Attendances = attendances,
                Permission = permission
            };

            return viewModel;
        }

        public List<StudentAttendanceViewModel> GetStudentAttendancedate(int studentId, DateOnly date)
        {
            // Retrieve student details
            var student = studentRepo.GetStudentById(studentId);
            if (student == null)
            {
                // Handle the case when student is not found
                return null;
            }

            // Retrieve attendance records for the specified student on the given date
            List<Attendence> attendances = studentRepo.GetStudentAttendances(studentId, date);

            // Create a list to store the view models
            List<StudentAttendanceViewModel> viewModels = new List<StudentAttendanceViewModel>();

            // Iterate over each attendance record
            foreach (var attendance in attendances)
            {
               
                var permission = studentRepo.GetPermissionByStudentId(studentId);
                // Create a new view model instance for each attendance record
                StudentAttendanceViewModel viewModel = new StudentAttendanceViewModel
                {
                    Id = student.Id,
                    // Include student details
                    Name = student.Name,
                    Email = student.Email,
                    Mobile = student.Mobile,
                    Degree=student.Degree,
                    University = student.University,
                    Faculty = student.Faculty,
                    GraduationYear = student.GraduationYear,
                    TrackName = student.Track?.Name, // Access track name if it's available

                    // Include attendance details
                    Attendances = attendances,
                    Permission = permission
                };

                // Add the view model to the list
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public List<StudentAttendanceViewModel> GetTrackAttendancedate(int trackId, DateOnly date)
        {
            // Retrieve all students in the given track
            List<Student> students = trackIRepo.GetTrackStudents(trackId);

            // Create a list to store attendance view models
            List<StudentAttendanceViewModel> viewModels = new List<StudentAttendanceViewModel>();

            // Iterate through each student to retrieve their attendance information
            foreach (var student in students)
            {
                // Retrieve the attendance information for the current student on the given date
                List<StudentAttendanceViewModel> studentAttendances = GetStudentAttendancedate(student.Id, date);

                // Add the attendance view models to the list
                viewModels.AddRange(studentAttendances);
            }

            return viewModels;
        }

    }
}
