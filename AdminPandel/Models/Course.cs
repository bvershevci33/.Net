using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public List<Student> Students { get; set; }
        public Profesor Profesor { get; set; }
    }

    public class Course1
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public List<Student> Students { get; set; }
    }
}
