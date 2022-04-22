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
        List<CourseDto> GetAll();
        List<CourseDto> GetAllAvailableByStudent(int studentId);
        void RegisterCourse(CourseRegistryDto dto);
    }
}
