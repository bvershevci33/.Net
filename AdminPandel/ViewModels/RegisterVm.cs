using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [MaxLength(255)]
        [Display(Name = "Emri")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Mbiemri")]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
        [Display(Name = "Datelindja")]
        public DateTime? BirthDay { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Emaili")]
        public string Email { get; set; }

        [MaxLength(255)]
        [DataType(DataType.Password)]
        [Display(Name ="Fjalekalimi")]
        public string Password { get; set; }

        [MaxLength(255)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password Propertia dhe Confirm Password nuk perputhen.")]
        [Display(Name ="Konfirmo Fjalekalimin")]
        public string ConfirmPassword { get; set; }
    }
}
