using School_Models.Data;
using School_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models.Repositories
{
    public class TeacherRepo : ITeacherRepo
    {
        private readonly SchoolDBContext context;
        public TeacherRepo(SchoolDBContext schoolDBContext) //niet Dbcontext!
        {
            this.context = schoolDBContext;
        }

        //TeacherRepository.cs
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await context.Teachers.OrderBy(t => t.Name)
                .Include(t=>t.TeachersEducations).ThenInclude(te=>te.Education)
                .ToListAsync();
        }
        
        public async Task<Teacher> GetTeacherForId(int id)
        {
            return await context.Teachers.Where(t => t.Id == id).SingleAsync();
        }

        public async Task<Teacher> Add(Teacher teacher)
        {
            try
            {
                var result = context.AddAsync(teacher);
                await context.SaveChangesAsync();

                return teacher;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void RemoveEducation(int teacherId)
        {
            var lst = context.TeachersEducations.Where(te => te.TeacherId == teacherId);
            context.TeachersEducations.RemoveRange(lst);
            context.SaveChanges();
        }

        public void RemoveTeacher(int teacherId)
        {
            var lst = context.Teachers.Where(te => te.Id == teacherId);
            context.Teachers.RemoveRange(lst);
            context.SaveChanges();
        }

        public void AddEducationsToTeacher(int teacherId, string[] educations)
        {
            foreach(var edu in educations)
            {
                context.TeachersEducations.Add(new TeachersEducations { TeacherId = teacherId, EducationId = Convert.ToInt32(edu) });
                context.SaveChanges();
            }
        }

        public async Task<Teacher> Update(Teacher t)
        {
            try
            {
                context.Teachers.Update(t); 

                await context.SaveChangesAsync();
                return t;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }
    }
}
