using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    public class StudentCourses
    {
        public int Id { get; set; }

        public List<Student> Students { get; set; }
        public int StudentId { get; set; }

        public List<Course1> Courses { get; set; }

        public int CourseId { get; set; }

        public int NumriOreve { get; set; }


    }
}
