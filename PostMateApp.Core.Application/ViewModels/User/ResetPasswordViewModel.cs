using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su nombre de usuario.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
