using CollegeApplication.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
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

        public void RegisterCourse(CourseRegistryDto dto)
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

        public List<CourseDto> GetAll()
        {
            var courses = _context.Courses
                .Select(c => new CourseDto 
                { 
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits
                })
                .ToList();

            if (!courses.Any())
                throw new Exception("There are no courses available");

            return courses;
        }

        public List<CourseDto> GetAllAvailableByStudent(int studentId) 
        {
            var courses = _context.Courses
                .Where(c => !c.Enrollments.Select(e => e.StudentId).Contains(studentId))
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits
                })
                .ToList();

            if (!courses.Any())
                throw new Exception("There are no courses available");

            return courses;
        }
    }
}
