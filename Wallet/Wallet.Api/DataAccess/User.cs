using System;
using System.Collections.Generic;

namespace Wallet.Api.DataAccess
{
    public partial class User
    {
        public User()
        {
            TransactionFromUsers = new HashSet<Transaction>();
            TransactionToUsers = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<Transaction> TransactionFromUsers { get; set; }
        public virtual ICollection<Transaction> TransactionToUsers { get; set; }
    }
}
