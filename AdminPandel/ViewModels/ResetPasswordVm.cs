using System.ComponentModel.DataAnnotations;

namespace AdminPandel.ViewModels
{
    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Shkruani fjalekalimin e njejte")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string  Token { get; set; }
    }
}
