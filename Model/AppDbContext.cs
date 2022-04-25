using Microsoft.EntityFrameworkCore;
using Model.Configurations;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new EnrollmentConfiguration());
            builder.Entity<Enrollment>().HasOne(e => e.Course).WithMany(c => c.Enrollments);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CollegeApp;Integrated Security=True");
        }
    }
}
