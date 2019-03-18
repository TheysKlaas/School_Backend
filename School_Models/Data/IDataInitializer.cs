using System.Collections.Generic;
using School_Models;

namespace School_Models.Data
{
    public interface IDataInitializer
    {
        IEnumerable<Education> Educations { get; set; }
        IEnumerable<Student> Students { get; set; }
        IEnumerable<Teacher> Teachers { get; set; }
    }
}