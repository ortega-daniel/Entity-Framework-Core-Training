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
    }
}
