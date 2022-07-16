using AdminPandel.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminPandel.ViewModels
{
    public class ProfesorViewModel
    {
        public int Id { get; set; }

        [Required()]
        [MaxLength(25)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required()]
        [MaxLength(25)]
        [MinLength(3)]
        public string LastName { get; set; }
        public decimal PagaNeto { get; set; }

        public IFormFile Image { get; set; }

        public IFormFileCollection Images { get; set; }

        public IFormFileCollection IMAGE { get; set; }

        public List<Student> Students { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
