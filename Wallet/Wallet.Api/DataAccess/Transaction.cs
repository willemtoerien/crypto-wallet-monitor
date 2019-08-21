using System;
using System.Collections.Generic;

namespace Wallet.Api.DataAccess
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public DateTime TransactionAt { get; set; }
        public bool IsSuspicious { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
