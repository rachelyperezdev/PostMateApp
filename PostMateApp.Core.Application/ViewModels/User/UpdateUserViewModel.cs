using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar su nombre de usuario.")]
        [DataType(DataType.Text)]
        [UniqueUsername(ErrorMessage = "Ese nombre de usuario ya está en uso. Por favor, elija otro.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben de coincidir.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe ingresar su nombre.")]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Debe ingresar su apellido.")]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Debe ingresar su número telefónico.")]
        [RegularExpression(@"^(1\s?-?\s?(809|829|849)|809|829|849)\s?-?\s?\d{3}\s?-?\s?\d{4}$", ErrorMessage = "Ingrese un número telefónico válido para República Dominicana.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Debe ingresar su email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string? ProfileImg { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
