using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wallet.Api.DataAccess;

namespace Wallet.Api.Areas.Transactions.Services
{
    public class TransactionsService
    {
        private readonly WalletDbContext db;
        private readonly TransactionsOptions options;

        public TransactionsService(WalletDbContext db, IOptions<TransactionsOptions> options)
        {
            this.db = db;
            this.options = options.Value;
        }

        public async Task<bool> IsSuspiciousAsync(Transaction transaction)
        {
            return await IsAmountAboveThresholdAsync(transaction.Amount) ||
                   IsAbovePredefinedAmount(transaction.Amount) || IsPurposeSuspicious(transaction.Purpose);
        }

        public async Task<bool> IsAmountAboveThresholdAsync(decimal amount)
        {
            var averageAmount = await db.Transactions.AverageAsync(x => x.Amount);
            var threshold = averageAmount * options.AverageAmountThresholdPercentage;
            return amount > averageAmount + threshold;
        }

        public bool IsAbovePredefinedAmount(decimal amount)
        {
            return amount > options.PredefinedAmount;
        }

        public bool IsPurposeSuspicious(string purpose)
        {
            return !options.NonSuspiciousPurposes.Any(x => x.Equals(purpose, StringComparison.OrdinalIgnoreCase));
        }
    }
}
