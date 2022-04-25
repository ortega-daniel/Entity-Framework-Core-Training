using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Abstractions
{
    public interface ICourseService
    {
        void Delete(DeleteCourseDto dto);
        List<CourseDto> GetAll();
        List<CourseDto> GetAllAvailableByStudent(int studentId);
        CourseDto GetById(int courseId);
        void RegisterCourse(CourseRegistryDto dto);
        void Update(CourseUpdateDto courseUpdate);
    }
}
