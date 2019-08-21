using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.Api.Areas.Transactions.Requests
{
    public class PayRequest
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public decimal Amount { get; set; }
    }
}
