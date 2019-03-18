using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_Models.Repositories
{
    public interface IEducationRepo
    {
        Task<Education> Create(Education education);
        Task Delete(Education education);
        Task<bool> EducationExists(int id);
        Task<IEnumerable<Education>> GetAllEducationsAsync(string search = null);
        Task<Education> GetEducationAsync(int id);
        IEnumerable<Education> GetAllEducationsByTeacher(int teacherId);
        Task<Education> GetEducationByCodeAsync(string code);
        Task<Education> Update(Education education);
    }
}