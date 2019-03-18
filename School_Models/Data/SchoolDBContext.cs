using School_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models.Data
{
    public class SchoolDBContext : DbContext
    {
        //ctor met opties(!)
        public SchoolDBContext(DbContextOptions<SchoolDBContext> options)
        : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<News> News { get; set; }
        //nodig voor de veel op veel relatie
        public DbSet<TeachersEducations> TeachersEducations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //composite key
            modelBuilder.Entity<TeachersEducations>()
            .HasKey(t => new { t.TeacherId, t.EducationId });

            //tabelnaam aanpassen
            modelBuilder.Entity<Teacher>().Property(t => t.Birthday).HasColumnName("DateOfBirth");
            modelBuilder.Entity<Student>().Property(t => t.Birthday).HasColumnName("DateOfBirth");

            modelBuilder.Entity<Teacher>().Ignore(t => t.ImageUrl);
        }
    }
}
