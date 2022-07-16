using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    public class Student
    {

        public int Id { get; set; }

        [MaxLength(25)]
        [Required(ErrorMessage = "Fusha Emri eshte e kerkuar.")]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string LastName { get; set; }

        [Required()]
        public int Age { get; set; }

        public byte[] Photo { get; set; }

        [Required()]
        public string Address { get; set; }

        [Required()]
        [Comment("1-Aktiv, 3-Pasiv")]
        public StudentStatus StatusiStudentit { get; set; }

        public Profesor Profesor { get; set; }

        [ForeignKey("Id")]
        public int ProfesorId { get; set; }

        public List<Course> Courses { get; set; }

            





    }

    public enum StudentStatus
    {
        Aktiv = 1,
        Pasiv = 3
    }
}
