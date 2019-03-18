using Microsoft.AspNetCore.Mvc.Rendering;
using School_Models;
using School_Models.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School_Webapp.ViewModels
{
    public class TeacherEducationsVM
    {
        public Teacher Teacher { get; set; }
        [Display(Name = "Eén of meerdere opleidingen")]
        public MultiSelectList Educations { get; set; }
        public IEnumerable<Education> SelectedEducations { get; set; }
        [Required(ErrorMessage = "Kies minstens één opleiding ")] public string[] SelectedEducationsString { get; set; } //helper voor HTTP

        public TeacherEducationsVM(IEducationRepo educationRepo, Teacher teacher)
        {
            this.Teacher = teacher;
            this.SelectedEducations = educationRepo.GetAllEducationsByTeacher(Teacher.Id);
            this.Educations = new MultiSelectList(educationRepo.GetAllEducationsAsync().Result, "Id", "Name", SelectedEducations);
        }

        public TeacherEducationsVM()
        {

        }
    }
}
