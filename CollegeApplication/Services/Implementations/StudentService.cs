using CollegeApplication.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entities;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context = new();

        public void RegisterStudent(StudentRegistryDto dto) 
        {
            try
            {
                _context.Students.Add(new Student(dto.CodeNumber, dto.FirstName, dto.LastName));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public StudentDto GetByCodeNumber(string codeNumber) 
        {
            var student = _context.Students
                .Select(s => new StudentDto 
                { 
                    Id = s.Id,
                    CodeNumber = s.CodeNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                })
                .FirstOrDefault(s => s.CodeNumber.ToLower().Equals(codeNumber));

            if (student is null)
                throw new Exception("Student doest not exist");

            return student;
        }

        public void AssignCourse(CourseAssignmentDto courseAssignment)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.CodeNumber.ToLower().Equals(courseAssignment.StudentCodeNumber));

            if (student is null)
                throw new ArgumentNullException("Student does not exist");

            var course = _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefault(c => c.Id.Equals(courseAssignment.CourseId));

            if (course is null)
                throw new ArgumentNullException("Course does not exist");

            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId.Equals(student.Id) && e.CourseId.Equals(course.Id));

            if (enrollment is not null)
            {
                if (enrollment.IsActive)
                {
                    throw new Exception("Student is already enrolled in this course");
                }
                else
                {
                    if (course.Enrollments.Where(e => e.IsActive).ToList().Count >= course.Capacity)
                        throw new Exception("Course is full");

                    enrollment.IsActive = true;
                }
            }
            else 
            {
                var newEnrollment = _context.Enrollments.Add(new Enrollment(course.Id, student.Id));
                student.Enrollments.Add(newEnrollment.Entity);
                course.Enrollments.Add(newEnrollment.Entity);
            }

            _context.SaveChanges();
        }

        public List<Enrollment> GetEnrollments(int studentId)
        {
            var enrollments = _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Where(e => e.IsActive)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToList();

            if (!enrollments.Any())
                throw new Exception("Student is not enrolled in any courses");

            return enrollments;
        }

        public void AssignGrade(List<GradeAssignmentDto> assignments) 
        {
            if (!assignments.Any()) 
                throw new ArgumentNullException("Assignments list is required");

            foreach (var assignment in assignments)
            {
                var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId.Equals(assignment.StudentId) && e.CourseId.Equals(assignment.CourseId) && e.IsActive);

                if (enrollment is null)
                    throw new Exception("Student is not enrolled in this course");

                enrollment.Grade = assignment.Grade;
            }

            _context.SaveChanges();
        }

        public StudentEvaluationDto GetEvaluationByCodeNumber(string codeNumber) 
        {
            var enrollments = _context.Enrollments
                .Where(e => e.Student.CodeNumber.ToLower().Equals(codeNumber.ToLower()))
                .Select(e => new StudentEnrollmentDto 
                { 
                    CourseId = e.Course.Id,
                    Title = e.Course.Title,
                    Grade = e.Grade
                })
                .ToList();

            var evaluation = _context.Students
                .Where(s => s.CodeNumber.ToLower().Equals(codeNumber.ToLower()))
                .Select(s => new StudentEvaluationDto 
                { 
                    Student = new StudentDto 
                    { 
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        CodeNumber = s.CodeNumber
                    },
                    Enrollments = enrollments
                })
                .FirstOrDefault();

            if (evaluation is null)
                throw new ArgumentNullException("Student does not exist");

            if (!enrollments.Any())
                throw new Exception("Student is not enrolled in any courses, or has been evaluated already");

            return evaluation;
        }

        public void Evaluate(StudentEvaluationDto evaluation) 
        {
            var enrollments = _context.Enrollments.Where(e => e.StudentId.Equals(evaluation.Student.Id)).ToList();

            try
            {
                foreach (var enrollment in enrollments)
                {
                    var x = evaluation.Enrollments.First(e => e.CourseId.Equals(enrollment.CourseId));
                    enrollment.Grade = x.Grade;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DropStudentCourse(DropStudentCourseDto dto) 
        {
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId.Equals(dto.StudentId) && e.CourseId.Equals(dto.CourseId) && e.IsActive);

            if (enrollment == null)
                throw new Exception("Student is not enrolled in this course");

            enrollment.IsActive = false;
            _context.SaveChanges();
        }
    }
}
