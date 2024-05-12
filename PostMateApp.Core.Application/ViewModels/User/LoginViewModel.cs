using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su nombre de usuario.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Debe ingresar su contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool HasError { get; set; }  
        public string? Error { get; set; }
    }
}
