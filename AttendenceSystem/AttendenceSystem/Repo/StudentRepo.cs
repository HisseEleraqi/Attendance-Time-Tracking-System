﻿using AttendenceSystem.Data;
using AttendenceSystem.IRepo;
using AttendenceSystem.Models;
using AttendenceSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{

    public class StudentRepo:IStudentRepo

    {
        private readonly DataContext context = new DataContext();

        private readonly TrackIRepo trackIRepo;
        public StudentRepo( TrackIRepo _trackIRepo)
        {
            
            trackIRepo = _trackIRepo;
        }
        public List<Student> GetAllStudents()
        {
            return context.Students.ToList();
        }
        public void UpdateStudent(Student student)
        {
            context.Students.FirstOrDefault(s=>s.Id==student.Id);
        }
        public void AddStudent(Student student)
        {
       
            student.IsAccepted = false;
            student.Degree = 255;
            
            context.Students.Add(student);
             context.SaveChanges();
            int studentRoleId = context.Roles.FirstOrDefault(S=>S.RoleName=="Student").Id; // Replace this with your logic to get the role ID

            // Create a new UserRole instance for the student
            var userRole = new UserRole
            {
                UserId = student.Id, 
                RoleId = studentRoleId  
            };

            // Add the userRole to the UserRole table
            context.UserRoles.Add(userRole);
            context.SaveChanges();
        }


          public Student GetStudentById(int userId)
        {

            return context.Students.FirstOrDefault(s => s.Id == userId);

        }




        public Schedule StudentSchedule(int studentId)
        {
            var schedule = context.Students
                                  .Where(s => s.Id == studentId)
                                  .SelectMany(s => s.Track.Schedules)
                                 
                                  .OrderByDescending(sch => sch.Date)
                                  .FirstOrDefault(); 

            return schedule;

        }
        public int GetStudentLateDays(int studentId)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today); 
            return context.Attendences.Count(s => s.IsLate && s.UserId == studentId && s.Date <= startDate);
        }

        public int GetStudentAbsentDays(int studentId)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today); 
            return context.Attendences.Count(s => s.IsAbsent && s.UserId == studentId && s.Date <= startDate);
        }
        public int GetStudentDegrees(int StudentId)
        {
            var degree = context.Students.FirstOrDefault(s => s.Id == StudentId).Degree;
            var LateMinus = GetStudentLateDays(StudentId)*5;
            var AbsentMinus=GetStudentAbsentDays(StudentId)*10;
            return degree - (LateMinus + AbsentMinus);
        }
        public List<Permision>GetStudentPermision(int StudentId)
        {
           return context.Permisions.Where(p=>p.StudentId==StudentId).ToList();

        }
        public void Addnewpermision(Permision newpermision)
        {
            context.Permisions.Add(newpermision);
            context.SaveChanges();

        }

        public void Deletpermision(int permisionId)
        {
            var permision=context.Permisions.FirstOrDefault(p => p.Id == permisionId);
            context.Permisions.Remove(permision);
            context.SaveChanges();
        }


      

        public List<Attendence> GetAttendancesByStudentId(int studentId)
        {
            return context.Attendences.Where(a => a.UserId == studentId).ToList();
        }

        public Permision GetPermissionByStudentId(int studentId)
        {
            return context.Permisions.FirstOrDefault(p => p.StudentId == studentId);
        }

        public List<Attendence> GetStudentAttendances(int studentId, DateOnly date)
        {
            
            List<Attendence> attendances = context.Attendences.Where(a => a.UserId == studentId && a.Date == date).ToList();

            return attendances;
        }
        public StudentAttendanceViewModel UpdateStudentDegree(int id, int newDegree)
        {
            var existingStudent = context.Students.FirstOrDefault(s => s.Id == id);

            if (existingStudent != null)
            {
                existingStudent.Degree = newDegree;
                context.SaveChanges();

                var viewModel = new StudentAttendanceViewModel
                {
                    Id = existingStudent.Id,
                    Name = existingStudent.Name,
                    Email = existingStudent.Email,
                    Mobile = existingStudent.Mobile,
                    University = existingStudent.University,
                    Faculty = existingStudent.Faculty,
                    GraduationYear = existingStudent.GraduationYear,
                    Degree = existingStudent.Degree, // Update the Degree property
                    TrackName = existingStudent.Track?.Name, // Access track name if it's available
                   
                };

                return viewModel;
            }

            return null; // Return null if student not found
        }



    }
}
