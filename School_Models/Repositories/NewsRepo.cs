using Microsoft.EntityFrameworkCore;
using School_Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Models.Repositories
{
    public class NewsRepo : INewsRepo
    {
        private readonly SchoolDBContext context;

        public NewsRepo(SchoolDBContext context)
        {
            this.context = context;
        }

        //READ --------------------------
        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            IEnumerable<News> result = null;

            result = await context.News.ToListAsync();
            

            return result.OrderBy(e => e.NewsHeader);
        }
    }
}
