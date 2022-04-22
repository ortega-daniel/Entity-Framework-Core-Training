using CollegeApplication.Services.Abstractions;
using Model;
using Model.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context = new();

        public void RegisterStudent(CourseRegistryDto dto)
        {
            try
            {
                _context.Courses.Add(new Course(dto.Title, dto.Credits));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
