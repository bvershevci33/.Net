using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(255)]
        public string FristName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
