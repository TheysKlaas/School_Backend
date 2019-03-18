using School_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models.Data
{
    public class DataInitializer:IDataInitializer 
    {
        public DataInitializer()
        {
            this.Educations = _educations.OrderBy(e => e.Code);
            this.Students = this.CreateFakeStudents(10);
        }

        //properties opvraagbaar bij IoC:
        public IEnumerable<Education> Educations { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }

        //initialisations op private :
        private List<Education> _educations = new List<Education>  {
           new Education {Id = 1,Code = "MCT", Name = "Media and Communication Technology"},
             new Education {Id =2,Code ="DEV", Name = "Degital Design and Development" },
             new Education {Id = 3, Code ="DAE", Name = "Digital Arts and Entertainment" }
        };

        private List<Student> _students = new List<Student>();
        private List<Student> CreateFakeStudents(int nmbrStudents)
        {
            
            for (var i = 1; i <= nmbrStudents; i++)
            {
                Student student = new Student();
                student.Id = i;
                student.Name = "nameStudent" + i;
                student.Email = "emailStudent" + i;
                student.Password = "pwdStudent" + i;
                student.Gender = (Person.GenderType) new Random().Next(0, 2);
                student.EducationId = new Random().Next(1, _educations.Count());
                student.Education = Educations.SingleOrDefault(e => e.Id == student.EducationId);

                _students.Add(student);
            }
            return this._students;
        }
    }
}