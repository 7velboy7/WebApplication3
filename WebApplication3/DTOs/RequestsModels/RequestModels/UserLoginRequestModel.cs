using System.ComponentModel.DataAnnotations;

namespace WebApplication3.RequestsModels.RequestModels
{
    public class UserLoginRequestModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
