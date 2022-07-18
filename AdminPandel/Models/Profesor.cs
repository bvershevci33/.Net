using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    //[Table("Profesoret", Schema ="users")]
    public class Profesor
    {
        public int Id { get; set; }

        [Required()]
        [MaxLength(25)]
        [Column("Emri")]
        public string FirstName { get; set; }

        [Required()]
        [Column(TypeName = "nvarchar(200)")]
        [Comment("Mbiemri i Profesorit")]
        public string LastName { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal PagaNeto { get; set; }
        
        public string ImgFilePath { get; set; }

        public List<Student> Students { get; set; }

        public Course Course { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

        public List<PersonImg> PersonImgs { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }


    }
}
