using System.Collections.Generic;
using System.Threading.Tasks;
using School_Models;

namespace School_Models.Repositories
{
    public interface ITeacherRepo
    {
        Task<Teacher> Add(Teacher teacher);
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher> GetTeacherForId(int id);
        void RemoveEducation(int teacherId);
        void RemoveTeacher(int teacherId);
        Task<Teacher> Update(Teacher t);
        void AddEducationsToTeacher(int teacherId, string[] educations);
    }
}