using System.ComponentModel.DataAnnotations;

namespace EduHome.App.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       public bool isRememberMe { get; set; }

    }
}
