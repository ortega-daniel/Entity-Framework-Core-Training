using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class CourseAssignmentDto
    {
        public string StudentCodeNumber { get; set; }
        public int CourseId { get; set; }

        public void ValidateCourseTitle() 
        {   
            if (CourseId <= 0)
                throw new ArgumentNullException("'CourseId' must be greater than 0");
        }

        public void ValidateStudentCodeNumber() 
        {
            if (string.IsNullOrEmpty(StudentCodeNumber))
                throw new ArgumentNullException("'StudentCodeNumber' is required");
        }
    }
}
