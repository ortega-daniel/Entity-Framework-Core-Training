using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }
        public bool IsActive { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

        public Enrollment() { }

        public Enrollment(int courseId, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
        }
    }
}
