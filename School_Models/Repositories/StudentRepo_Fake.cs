using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Models.Data;
using School_Models;

namespace School_Models.Repositories
{
    public class StudentRepo_Fake
    {
        private readonly IDataInitializer context;

        public StudentRepo_Fake(IDataInitializer fake_context)
        {
            context = fake_context;
        }

        public Task<Student> AddAsync(Student student)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var result = context.Students.OrderBy(student => student.Name);
            return await Task.FromResult(result);
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            var student = context.Students.SingleOrDefault(s => s.Id == id);
            return await Task.FromResult(student);

        }

        public Task<IEnumerable<Student>> GetStudentsByEducationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
