using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models
{
    public class TeachersEducations
    {
        [Key]
        public int TeacherId { get; set; }
        [Key]
        public int EducationId { get; set; }
        //navigatie properties - many to many
        public Teacher Teacher { get; set; }
        public Education Education { get; set; }



    }
}
