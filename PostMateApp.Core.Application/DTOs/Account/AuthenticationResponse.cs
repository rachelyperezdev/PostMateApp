using PostMateApp.Core.Application.DTOs.Account.Common;

namespace PostMateApp.Core.Application.DTOs.Account
{
    public class AuthenticationResponse : BaseResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public List<string> Roles { get; set; }
    }
}
