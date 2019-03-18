using Microsoft.AspNetCore.Mvc.Rendering;
using School_Models;
using School_Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Webapp.ViewModels
{
    public class StudentEducationVm
    {
        public StudentEducationVm(IEducationRepo educationRepo, Student student)
        {
            this.Student = student;
            //Selectlist houdt geselecteerde waarde bij.(bij edit indien EducationId)
            this.Education = new SelectList(educationRepo.GetAllEducationsAsync(null).Result, "Id", "Name", student.EducationId);
        }
        public StudentEducationVm()
        {

        }
        public Student Student { get; set; }
        public SelectList Education { get; private set; }
    }
}
