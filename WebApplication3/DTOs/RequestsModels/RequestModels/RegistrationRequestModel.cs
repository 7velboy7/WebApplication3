using System.ComponentModel.DataAnnotations;

namespace WebApplication3.RequestsModels.RequestModels
{
    public class RegistrationRequestModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
