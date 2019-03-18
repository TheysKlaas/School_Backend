using System.Collections.Generic;
using System.Threading.Tasks;

namespace School_Models.Repositories
{
    public interface INewsRepo
    {
        Task<IEnumerable<News>> GetAllNewsAsync();
    }
}