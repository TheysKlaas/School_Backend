using System;
using System.Collections.Generic;
using System.Text;

namespace School_Models
{
    public class News
    {
        public int Id { get; set; }
        public string NewsHeader { get; set; }
        public string NewsBody { get; set; }
        public DateTime DateOfEntry { get; set; }
    }
}
