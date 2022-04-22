using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentRegistryDto
    {
        public string CodeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Validation() 
        {
            if (string.IsNullOrEmpty(CodeNumber))
                throw new ArgumentNullException("Name can't be left empty");

            if (string.IsNullOrEmpty(FirstName))
                throw new ArgumentNullException("Name can't be left empty");

            if (string.IsNullOrEmpty(LastName))
                throw new ArgumentNullException("Name can't be left empty");
        }
    }
}
