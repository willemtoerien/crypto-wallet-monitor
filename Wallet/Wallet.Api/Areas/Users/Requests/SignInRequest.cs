using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Areas.Users.Requests
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
