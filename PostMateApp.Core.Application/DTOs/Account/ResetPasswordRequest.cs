namespace PostMateApp.Core.Application.DTOs.Account
{
    public class ResetPasswordRequest
    {
        public string Username { get; set; }
        public string? Token { get; set; }
    }
}
