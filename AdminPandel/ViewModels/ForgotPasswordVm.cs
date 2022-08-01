using System.ComponentModel.DataAnnotations;

namespace AdminPandel.ViewModels
{
    public class ForgotPasswordVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
