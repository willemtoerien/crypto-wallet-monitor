namespace Wallet.Api.Areas.Users.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }

        public UserInfo(DataAccess.User user)
        {
            UserId = user.UserId;
            Email = user.Email;
            Balance = user.Balance;
        }
    }
}
