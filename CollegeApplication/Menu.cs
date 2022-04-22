using CollegeApplication.Services.Abstractions;
using CollegeApplication.Services.Implementations;
using Model.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication
{
    public class Menu
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public Menu()
        {
            _studentService = new StudentService();
            _courseService = new CourseService();
        }

        public bool Show() 
        {
            Console.Clear();
            Console.WriteLine("Choose an option");
            Console.WriteLine("1) Student registration");
            Console.WriteLine("2) Course registration");
            Console.WriteLine("3) Course enrollment");
            Console.WriteLine("4) Evaluate student performance");
            Console.WriteLine("5) View student performance");
            Console.WriteLine("0) Exit");
            Console.Write("\nYour option:");

            switch (Console.ReadLine())
            {
                case "0":
                    return false;
                case "1":
                    Console.Clear();
                    RegisterStudent();
                    break;
                case "2":
                    Console.Clear();
                    RegisterCourse();
                    break;
                default:
                    break;
            }

            return true;
        }

        private void RegisterStudent() 
        {
            StudentRegistryDto studentRegistry = new();

            Console.WriteLine("Please enter the required information to register a new student");
            Console.Write("Student Code Number: ");
            studentRegistry.CodeNumber = Console.ReadLine();
            Console.Write("First Name: ");
            studentRegistry.FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            studentRegistry.LastName = Console.ReadLine();

            try
            {
                studentRegistry.Validation();
                _studentService.RegisterStudent(studentRegistry);
                Console.WriteLine("Student registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void RegisterCourse() 
        {
            CourseRegistryDto courseRegistry= new();

            Console.WriteLine("Please enter the required information to register a new student");
            Console.Write("Title: ");
            courseRegistry.Title = Console.ReadLine();
            Console.Write("Credits: ");
            string creditsInput = Console.ReadLine();

            if (!Int32.TryParse(creditsInput, out int credits)) 
            {
                Console.Write("Please enter a numeric value for 'Credits'\nPress enter to continue");
                Console.ReadLine();
                return;
            }

            try
            {
                courseRegistry.Credits = credits;
                courseRegistry.Validation();
                _courseService.RegisterStudent(courseRegistry);
                Console.WriteLine("Course registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }
        }
    }
}
