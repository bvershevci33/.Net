using AdminPandel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.ViewModels
{
    public class StudentVm
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

        [Required()]
        public string Address { get; set; }

        [Required()]
        [Comment("1-Aktiv, 3-Pasiv")]
        public StudentStatus StatusiStudentit { get; set; }
        
        public SelectList Profesors { get; set; }

  
        public int ProfesorId { get; set; }

        public IFormFile Photo { get; set; }

        public string Basae64Img { get; set; }

        //public SelectList Courses { get; set; }

        //public int CourseId { get; set; }

    }

}
