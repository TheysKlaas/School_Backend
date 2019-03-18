using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models
{
    public class Student:Person
    {
        //[DefaultValue("Geen education gekozen")]
        public int EducationId { get; set; }

        //navigatie property - one to many
        public Education Education { get; set; }
           

    }
}
