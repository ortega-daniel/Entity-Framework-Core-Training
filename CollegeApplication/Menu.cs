using CollegeApplication.Services.Abstractions;
using CollegeApplication.Services.Implementations;
using Model.Entities;
using Shared.Dtos;
using Shared.Enums;
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
            Console.WriteLine("6) Edit course");
            Console.WriteLine("7) Drop student");
            Console.WriteLine("8) Delete course");
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
                case "3":
                    Console.Clear();
                    AssignCourse();
                    break;
                case "4":
                    Console.Clear();
                    AssignGrade();
                    break;
                case "5":
                    Console.Clear();
                    ViewStudentGrades();
                    Console.ReadLine();
                    break;
                case "6":
                    Console.Clear();
                    UpdateCourse();
                    break;
                case "7":
                    Console.Clear();
                    DropStudentCourse();
                    break;
                case "8":
                    Console.Clear();
                    DeleteCourse();
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
                Console.Write("Press enter to continue");
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
                _courseService.RegisterCourse(courseRegistry);
                Console.WriteLine("Course registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void AssignCourse() 
        {
            var courseAssignment = new CourseAssignmentDto();

            Console.WriteLine("Please enter the required information to assign a course");
            Console.Write("Student Code Number: ");

            try
            {
                courseAssignment.StudentCodeNumber = Console.ReadLine();
                courseAssignment.ValidateStudentCodeNumber();

                var student = _studentService.GetByCodeNumber(courseAssignment.StudentCodeNumber);
                var courses = _courseService.GetAllAvailableByStudent(student.Id);

                Console.WriteLine($"\nStudent Information\nStudent code number: {student.CodeNumber}\tName: {student.FirstName} {student.LastName}");
                
                Console.WriteLine("\nAvailable courses");
                foreach (var course in courses)
                {
                    string capacity = course.Capacity != 0 ? $"{course.Capacity} spots left" : "Full";
                    Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}\tCredits: {course.Credits}\tCapacity: {capacity}");
                }
                
                Console.WriteLine("\nPlease choose a course");
                Console.Write("Course Id: ");
                string input = Console.ReadLine();

                if (!Int32.TryParse(input, out int courseId)) 
                    throw new Exception("'CourseId' must be a numeric value");

                courseAssignment.CourseId = courseId;
                courseAssignment.ValidateCourseTitle();

                _studentService.AssignCourse(courseAssignment);
                Console.WriteLine("Successful Enrollment");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void AssignGrade() 
        {
            Console.WriteLine("Please enter the required information to assign a grade");
            Console.Write("Student Code Number: ");

            try
            {
                string studentCodeNumber = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(studentCodeNumber))
                    throw new Exception("'Student Code Number' is required");

                var student = _studentService.GetByCodeNumber(studentCodeNumber);
                var enrollments = _studentService.GetEnrollments(student.Id);

                Console.WriteLine($"\nStudent Information\nStudent code number: {student.CodeNumber}\tName: {student.FirstName} {student.LastName}");

                Console.WriteLine("\nEnrolled Courses");
                foreach (var enrollment in enrollments)
                    Console.WriteLine($"Id: {enrollment.Course.Id}\tTitle: {enrollment.Course.Title}\tGrade: {enrollment.Grade}");

                List<GradeAssignmentDto> assignments = new();

                while (true) 
                {
                    Console.Write("\nCourse Id: ");
                    string input = Console.ReadLine().Trim();

                    if (!Int32.TryParse(input, out int courseId))
                        throw new Exception("Course Id must be a numeric value");

                    var grades = Enum.GetNames(typeof(Grade));
                    string availableGrades = String.Join(", ", grades);

                    Console.Write($"Select grade ({availableGrades}): ");
                    string inputGrade = Console.ReadLine();

                    if (!Enum.TryParse(inputGrade, out Grade grade))
                        throw new Exception("Grade value is invalid");

                    assignments.Add(new GradeAssignmentDto { StudentId = student.Id, CourseId = courseId, Grade = grade});

                    Console.WriteLine("Continue adding products? (y/n): ");
                    if (Console.ReadLine().Trim().ToLower() != "y")
                        break;
                }

                _studentService.AssignGrade(assignments);
                Console.WriteLine("Grades assigned successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void ViewStudentGrades() 
        {
            Console.WriteLine("Please enter the required information to assign a grade");
            Console.Write("Student Code Number: ");

            try
            {
                string studentCodeNumber = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(studentCodeNumber))
                    throw new Exception("'Student Code Number' is required");

                var student = _studentService.GetByCodeNumber(studentCodeNumber);
                var enrollments = _studentService.GetEnrollments(student.Id);

                Console.WriteLine($"\nStudent Information\nStudent code number: {student.CodeNumber}\tName: {student.FirstName} {student.LastName}");

                Console.WriteLine("\nStudent Performance");
                foreach (var enrollment in enrollments)
                    Console.WriteLine($"Id: {enrollment.Course.Id}\tTitle: {enrollment.Course.Title}\tGrade: {enrollment.Grade}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private StudentEvaluationDto ConsultStudentPeformance() 
        {
            Console.WriteLine("Please enter the required information to view the student performance");
            Console.Write("Student Code Number: ");
            var input = Console.ReadLine();

            try
            {
                var evaluation = _studentService.GetEvaluationByCodeNumber(input);

                Console.WriteLine($"\nStudent Information\nStudent code number: {evaluation.Student.CodeNumber}\tName: {evaluation.Student.FirstName} {evaluation.Student.LastName}");
                Console.WriteLine("\nEnrolled courses");

                foreach (var enrollment in evaluation.Enrollments)
                {
                    var grade = enrollment.Grade is null ? "-" : enrollment.Grade.ToString();
                    Console.WriteLine($"{enrollment.CourseId}. {enrollment.Title}\t\t{grade}");
                }

                return evaluation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void EvaluateStudentPerformance() 
        {
            var con = false;
            var isValidUpdate = false;
            var evaluation = ConsultStudentPeformance();

            if (evaluation is null)
                return;

            Console.WriteLine($"\nValid grades: {string.Join(", ", Enum.GetNames(typeof(Grade)))}");

            do
            {
                Console.WriteLine("\nPlease select the course you wish to grade");
                Console.Write("Course Id: ");
                var inputId = Console.ReadLine();

                var isValid = Int32.TryParse(inputId, out int id);

                if (!isValid) 
                {
                    Console.WriteLine("Course Id must be a numeric value");
                    return;
                }

                var exists = evaluation.Enrollments.Exists(e => e.CourseId.Equals(id));

                if (!exists) 
                {
                    Console.WriteLine("Student is not enrolled in this course");
                    return;
                }

                Console.WriteLine("Grade: ");
                var inputGrade = Console.ReadLine();

                isValid = Enum.TryParse(inputGrade.ToUpper(), out Grade grade);

                if (!isValid)
                {
                    Console.WriteLine("Grade is invalid");
                    return;
                }

                int index = evaluation.Enrollments.FindIndex(e => e.CourseId.Equals(id));
                evaluation.Enrollments.ElementAt(index).Grade = grade;

                Console.Write("Assign another grade? (y/n): ");
                var input = Console.ReadLine();

                con = input.ToLower().Equals("y");

                if (!con)
                    isValidUpdate = true;
            } while (con);

            if (!isValidUpdate)
                return;

            _studentService.Evaluate(evaluation);
        }

        private void UpdateCourse() 
        {
            Console.WriteLine("All Courses");
            var courses = _courseService.GetAll();

            foreach (var course in courses)
                Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}\tCredits: {course.Credits}\tTotal Capacity: {course.Capacity}");

            Console.WriteLine("\nPlease enter the required information to update a course");
            CourseUpdateDto courseUpdate = new();

            try
            {
                Console.Write("Course Id: ");
                string input = Console.ReadLine().Trim();

                if (!Int32.TryParse(input, out int courseId))
                    throw new Exception("Course Id must be a numeric value");

                var course = _courseService.GetById(courseId);

                courseUpdate.Id = course.Id;
                Console.WriteLine("\nPlease enter the new values for course (empty=old value)");

                Console.Write("\nTitle: ");
                input = Console.ReadLine().Trim();
                courseUpdate.Title = string.IsNullOrEmpty(input) ? course.Title : input;

                Console.Write("Credits: ");
                input = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(input))
                {
                    courseUpdate.Credits = course.Credits;
                }
                else
                {
                    if (!Int32.TryParse(input, out int credits))
                        throw new Exception("Credits must be a numeric value");

                    courseUpdate.Credits = credits;
                }

                Console.Write("Capacity: ");
                input = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(input))
                {
                    courseUpdate.Capacity = course.Capacity;
                }
                else
                {
                    if (!Int32.TryParse(input, out int capacity))
                        throw new Exception("Capacity must be a numeric value");

                    courseUpdate.Capacity = capacity;
                }

                _courseService.Update(courseUpdate);
                Console.WriteLine("Course updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void DropStudentCourse() 
        {
            Console.WriteLine("Please enter the required information to drop a student");
            Console.Write("Student Code Number: ");

            try
            {
                string studentCodeNumber = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(studentCodeNumber))
                    throw new Exception("'Student Code Number' is required");

                var student = _studentService.GetByCodeNumber(studentCodeNumber);
                var enrollments = _studentService.GetEnrollments(student.Id);

                Console.WriteLine($"\nStudent Information\nStudent code number: {student.CodeNumber}\tName: {student.FirstName} {student.LastName}");

                Console.WriteLine("\nEnrolled Courses");
                foreach (var enrollment in enrollments)
                    Console.WriteLine($"Id: {enrollment.Course.Id}\tTitle: {enrollment.Course.Title}\tGrade: {enrollment.Grade}");

                Console.Write("\nCourse Id: ");
                string input = Console.ReadLine().Trim();

                if (!Int32.TryParse(input, out int courseId))
                    throw new Exception("Course Id must be a numeric value");

                _studentService.DropStudentCourse(new DropStudentCourseDto { StudentId = student.Id, CourseId = courseId });
                Console.WriteLine("Student dropped successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }

        private void DeleteCourse() 
        {
            Console.WriteLine("Please enter the required information to delete a course");

            try
            {
                var courses = _courseService.GetAll();

                foreach (var course in courses)
                    Console.WriteLine($"Id: {course.Id}\tTitle: {course.Title}");

                Console.Write("\nCourse Id: ");
                string input = Console.ReadLine().Trim();

                if (!Int32.TryParse(input, out int courseId))
                    throw new Exception("Course Id must be a numeric value");

                _courseService.Delete(new DeleteCourseDto { Id = courseId });
                Console.WriteLine("Course deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }
    }
}
