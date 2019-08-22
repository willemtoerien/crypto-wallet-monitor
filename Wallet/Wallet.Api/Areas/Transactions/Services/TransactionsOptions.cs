using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.Api.Areas.Transactions.Services
{
    public class TransactionsOptions
    {
        public int ThresholdBeginsAt { get; set; }

        public decimal AverageAmountThresholdPercentage { get; set; }

        public decimal PredefinedAmount { get; set; }

        public string[] NonSuspiciousPurposes { get; set; }
    }
}
