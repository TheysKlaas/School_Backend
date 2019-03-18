using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_Models.Repositories
{
    public interface IStudentRepo
    {
        Task<Student> Create(Student student);
        Task Delete(Student student);
        Task<IEnumerable<Student>> GetAllStudentsAsync(string search = null);
        Task<Student> GetStudentAsync(int id);
        Task<IEnumerable<Student>> GetStudentsByEducationAsync(int id);
        Task<bool> StudentExists(int id);
        Task<Student> Update(Student student);
    }
}