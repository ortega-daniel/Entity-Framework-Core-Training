using Model.Entities;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Abstractions
{
    public interface IStudentService
    {
        StudentDto GetByCodeNumber(string codeNumber);
        void RegisterStudent(StudentRegistryDto dto);
        void AssignCourse(CourseAssignmentDto courseAssignment);
        List<Enrollment> GetEnrollments(int studentId);
        void AssignGrade(List<GradeAssignmentDto> gradeAssignments);
        StudentEvaluationDto GetEvaluationByCodeNumber(string codeNumber);
    }
}
