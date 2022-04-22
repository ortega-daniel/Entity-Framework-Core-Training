using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppDbContextSeed
    {
        public static Task SeedAsync(AppDbContext context) 
        {
            if (!context.Students.Any()) 
            {
                var seedStudents = new List<Student>() 
                { 
                    new Student {CodeNumber = "S01246837", FirstName = "Mario", LastName = "Lopez" },
                    new Student {CodeNumber = "S01246838", FirstName = "Rene", LastName = "Quinonez" },
                    new Student {CodeNumber = "S01246839", FirstName = "Alejandra", LastName = "Flores" },
                };

                foreach (var student in seedStudents)
                {
                    context.Students.Add(student);
                }

                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                var seedCourses = new List<Course>()
                {
                    new Course { Title = "Chemistry", Credits = 6},
                    new Course { Title = "Spanish", Credits = 4},
                };

                foreach (var course in seedCourses)
                {
                    context.Courses.Add(course);
                }

                context.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
