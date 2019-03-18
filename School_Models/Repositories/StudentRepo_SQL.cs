using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using School_Models;
using Microsoft.Extensions.Configuration;
using School_Models.Data;
using Microsoft.EntityFrameworkCore;

namespace School_Models.Repositories
{
    public class StudentRepo_SQL : IStudentRepo
    {
        private readonly SchoolDBContext context;
        private readonly IEducationRepo educationRepo;

        public StudentRepo_SQL(SchoolDBContext context, IEducationRepo educationRepo)
        {
            this.context = context;
            this.educationRepo = educationRepo;
        }

        //READ --------------------------
        public async Task<IEnumerable<Student>> GetAllStudentsAsync(string search
        = null)
        {
            IEnumerable<Student> result = null;
            if (string.IsNullOrEmpty(search))
            {
                result = await context.Students.ToListAsync();
                foreach (Student s in result)
                {
                    s.Education = await educationRepo.GetEducationAsync(s.EducationId);
                }
            }
            else
            {
                var query = context.Students.Where(e => e.Name.Contains(search));
                result = await query.ToListAsync();
            }
            return result.OrderBy(e => e.Name);
        }

        public async Task<Student> GetStudentAsync(int id)
         => await context.Students.SingleAsync<Student>(e => e.Id == id);


        public async Task<IEnumerable<Student>> GetStudentsByEducationAsync(int id)
        {
            IEnumerable<Student> result = null;
            var query = context.Students.Where(e => e.EducationId.Equals(id));
            result = await query.ToListAsync();
            return result.OrderBy(e => e.Name);
        }

        //CREATE ------------------------

        public async Task<Student> Create(Student student)
        {
            try
            {
                var result = context.Students.AddAsync(student);
                await context.SaveChangesAsync();
                return student; //heeft nu een id (autoidentity)
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return null;
            }

            // SQL
            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    string SQL = "Insert into Students(Name, Gender, Email, DateOfBirth, PassWord , EducationId)";
            //    SQL += " Values(@Name, @Gender, @Email, @DateOfBirth, @PassWord , @EducationId)";

            //    SqlCommand cmd = new SqlCommand(SQL, con);
            //    cmd.Parameters.AddWithValue("@Name", student.Name);
            //    cmd.Parameters.AddWithValue("@Gender", student.Gender);
            //    cmd.Parameters.AddWithValue("@Email", student.Email ?? "");

            //    //cmd.Parameters.AddWithValue("@DateOfBirth", Student.DateOfBirth != null ? Student.DateOfBirth : (System.DateTime) SqlDateTime.Null);
            //    if (student.Birthday != null)
            //    {
            //        cmd.Parameters.AddWithValue("@DateOfBirth", student.Birthday);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@Password", student.Password ?? "");

            //    //Maak educationId  Nullable via int? ( gezien het een integer is) 
            //    if (student.EducationId != null)
            //    {
            //        cmd.Parameters.AddWithValue("@EducationId", student.EducationId);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@EducationId", DBNull.Value);
            //    }

            //    con.Open();
            //    //SqlDataReader reader = cmd.ExecuteReader();
            //    await cmd.ExecuteNonQueryAsync();
            //    //cmd.ExecuteNonQuery();
            //    con.Close();
            //    return student;
            //}
        }

        //UPDATE ------------------------
        public async Task<Student> Update(Student student)
        {
            try
            {
                context.Students.Update(student); //synchroon
                                                      ////asynchroon
                                                      //await Task.Factory.StartNew(() =>
                                                      //{
                                                      // context.Educations.Update(education);
                                                      //});
                await context.SaveChangesAsync();
                return student;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }


        //DELETE ------------------------
        public async Task Delete(Student student)
        {
            await Task.Factory.StartNew(() =>
            {
                context.Set<Student>().Remove(student);
            });
            await context.SaveChangesAsync();
        }

        //Helpers --------------------------
        public async Task<bool> StudentExists(int id)
        {
            return await context.Students.AnyAsync(e => e.Id == id);
        }

        //SQL helpers --------------------------------------------------
        //private async Task<List<Student>> GetData(SqlDataReader reader)
        //{
        //    List<Student> lst = new List<Student>();
        //    //1. try catch verhindert applicatie crash
        //    try
        //    {
        //        while (await reader.ReadAsync())
        //        {
        //            Student s = new Student();
        //            s.Id = Convert.ToInt32(reader["Id"]);
        //            s.Name = !Convert.IsDBNull(reader["Name"]) ? (string)reader["Name"] : "";
        //            //TO DO: verder uitbouwen van overige properties
        //            s.Email = !Convert.IsDBNull(reader["Email"]) ? (string)reader["Email"] : "";
        //            s.Password = !Convert.IsDBNull(reader["Password"]) ? (string)reader["Password"] : "";
        //            s.Gender = Convert.ToInt32(reader["Gender"]) == 0 ? Person.GenderType.Male : Person.GenderType.Female;
        //            if (!Convert.IsDBNull(reader["DateOfBirth"]) ){
        //                s.Birthday = Convert.ToDateTime(reader["DateOfBirth"]);
        //            }
        //            //EducationId kan NULL zijn.
        //            if (!Convert.IsDBNull(reader["EducationId"])) {
        //                s.EducationId = (int)reader["EducationId"];
        //              s.Education = await educationRepo.GetEducationAsync(s.EducationId.Value);
        //            }

        //            //Let op mogelijke NULL waarden (=> anders crash) 
        //            lst.Add(s);
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.Write(exc.Message); //later loggen
        //    }
        //    finally
        //    {
        //        reader.Close();  //Niet vergeten. Beperkt aantal verbindingen (of kosten)
        //    }
        //    return lst;
        //}


    }
}
