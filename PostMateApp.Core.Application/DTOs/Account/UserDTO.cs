using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.DTOs.Account
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? ProfileImg { get; set; }
    }
}
