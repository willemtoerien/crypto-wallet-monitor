using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wallet.Api.DataAccess;

namespace Wallet.Api.Areas.Transactions.Services
{
    public class TransactionsService
    {
        private readonly ILogger<TransactionsController> logger;
        private readonly WalletDbContext db;
        private readonly TransactionsOptions options;

        public TransactionsService(ILogger<TransactionsController> logger, WalletDbContext db, IOptions<TransactionsOptions> options)
        {
            this.logger = logger;
            this.db = db;
            this.options = options.Value;
        }

        public async Task<bool> IsSuspiciousAsync(Transaction transaction)
        {
            logger.LogInformation($"Checking if the transaction '{transaction.TransactionId}' is suspicious...");
            return await IsAmountAboveThresholdAsync(transaction.FromUserId, transaction.Amount) ||
                   IsAbovePredefinedAmount(transaction.Amount) || IsPurposeSuspicious(transaction.Purpose);
        }

        public async Task<bool> IsAmountAboveThresholdAsync(int fromUserId, decimal amount)
        {
            logger.LogDebug("Only start checking the averages aver a certain number of transactions.");
            if (await db.Transactions.CountAsync(x => x.FromUserId == fromUserId) < options.ThresholdBeginsAt)
            {
                return false;
            }
            logger.LogDebug("Obtain the average amount...");
            var averageAmount = await db.Transactions.Where(x => x.FromUserId == fromUserId).AverageAsync(x => x.Amount);
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
