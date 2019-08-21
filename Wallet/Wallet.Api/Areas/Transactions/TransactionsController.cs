using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Areas.Transactions.Requests;
using Wallet.Api.Areas.Transactions.Services;
using Wallet.Api.DataAccess;

namespace Wallet.Api.Areas.Transactions
{
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        private readonly TransactionsService transactionsService;

        public TransactionsController(WalletDbContext db, TransactionsService transactionsService) : base(db)
        {
            this.transactionsService = transactionsService;
        }

        [HttpGet("{userId}/out")]
        public async Task<Transaction[]> GetOutgoingTransactions(int userId)
        {
            return await Db.Transactions
                .Where(x => x.ToUserId == userId)
                .OrderByDescending(x => x.TransactionAt)
                .ToArrayAsync();
        }

        [HttpGet("{userId}/in")]
        public async Task<Transaction[]> GetIncomingTransactions(int userId)
        {
            return await Db.Transactions
                .Where(x => x.FromUserId == userId)
                .OrderByDescending(x => x.TransactionAt)
                .ToArrayAsync();
        }

        [HttpPost("pay")]
        public async Task Pay([FromBody] PayRequest request)
        {
            var transaction = new Transaction
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                Amount = request.Amount,
                TransactionAt = DateTime.Now,
                TransactionId = Guid.NewGuid()
            };
            transaction.IsSuspicious = await transactionsService.IsSuspiciousAsync(transaction);
            await Db.AddAsync(transaction);
            await Db.SaveChangesAsync();
        }
    }
}
