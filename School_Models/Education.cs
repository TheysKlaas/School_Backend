using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht met maximaal 10 karakters")]
        [StringLength(10, ErrorMessage = "De Code is maximaal 10 karakters.")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Beschrijving")]
        [MaxLength(300)]
        public string Description { get; set; }

        //navigatie property
        //-- has many
        public ICollection<Student> Students { get; set; }
        //-- many-to-many
        public ICollection<TeachersEducations> TeachersEducations { get; set; }
    }
}
