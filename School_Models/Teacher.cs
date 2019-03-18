using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models
{
    public class Teacher : Person
    {

        // public int EducationId { get; set; }

        //public ICollection<Education> Educations { get; set; }
        public ICollection<TeachersEducations> TeachersEducations { get; set; }
    }
}
