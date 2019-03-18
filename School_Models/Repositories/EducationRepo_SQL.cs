using School_Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using School_Models.Data;
using Microsoft.EntityFrameworkCore;

namespace School_Models.Repositories
{
    public class EducationRepo_SQL : IEducationRepo
    {
        private readonly SchoolDBContext context;
        private readonly INewsRepo newsRepo;

        public EducationRepo_SQL(SchoolDBContext context, INewsRepo newsRepo)
        {
            this.context = context;
            this.newsRepo = newsRepo;
        }

        //READ --------------------------
        public async Task<IEnumerable<Education>> GetAllEducationsAsync(string search = null)
        {
            IEnumerable<Education> result = null;
            //null moet expliciete gedeclareerd worden.
            //Of: List<Student> result2 = null;
            if (string.IsNullOrEmpty(search))
            {
                result = await context.Educations.ToListAsync();
                //of met LINQ:
                //await (from edu in context.Educations select edu).ToListAsync();
            }
            else
            {
                var query = context.Educations.Where(e => e.Name.Contains(search) |
                e.Code.Contains(search));
                result = await query.ToListAsync();
            }
            return result.OrderBy(e => e.Name);
        }

        public IEnumerable<Education> GetAllEducationsByTeacher(int teacherId)
        {
            // met linq
            var query = from te in context.TeachersEducations where te.TeacherId == teacherId from edu in context.Educations select te.Education;

            //Iqueryable met c#
            query = context.TeachersEducations.Include(te => te.Education).Where(te => te.TeacherId == teacherId).Select(te => te.Education);

            return query.ToList<Education>().Distinct<Education>();
        }

        public async Task<Education> GetEducationAsync(int id)
         //=> await context.Educations.FirstOrDefaultAsync<Education>(e => e.Id == id);
         //Of: enig (!) bestaande element returnen of een exceptie
         => await context.Educations.SingleAsync<Education>(e => e.Id == id);

        public async Task<Education> GetEducationByCodeAsync(string code)
         => await context.Educations
         .OrderByDescending(e => e.Code)
        .FirstOrDefaultAsync<Education>(e => e.Code.Contains(code));



        //CREATE ------------------------
        public async Task<Education> Create(Education education)
        {
            try
            {
                var result = context.Educations.AddAsync(education);
                await context.SaveChangesAsync();
                return education; //heeft nu een id (autoidentity)
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return null;
            }
        }

        //UPDATE ------------------------
        public async Task<Education> Update(Education education)
        {
            try
            {
                context.Educations.Update(education); //synchroon
                                                      ////asynchroon
                                                      //await Task.Factory.StartNew(() =>
                                                      //{
                                                      // context.Educations.Update(education);
                                                      //});
                await context.SaveChangesAsync();
                return education;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }

        //DELETE ------------------------
        public async Task Delete(Education education)
        {
            await Task.Factory.StartNew(() =>
            {
                context.Set<Education>().Remove(education);
            });
            //doe hier een archivering van education ipv delete -> veiliger
            await context.SaveChangesAsync();
        }

        //Helpers --------------------------
        public async Task<bool> EducationExists(int id)
        {
            return await context.Educations.AnyAsync(e => e.Id == id);
        }

    }
}