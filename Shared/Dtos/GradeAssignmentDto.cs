using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class GradeAssignmentDto
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }
    }
}
