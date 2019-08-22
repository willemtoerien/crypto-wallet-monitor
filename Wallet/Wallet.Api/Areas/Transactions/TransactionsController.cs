using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Api.Areas.Transactions.Requests;
using Wallet.Api.Areas.Transactions.Services;
using Wallet.Api.Areas.Users.Models;
using Wallet.Api.DataAccess;

namespace Wallet.Api.Areas.Transactions
{
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        private readonly ILogger<TransactionsController> logger;
        private readonly TransactionsService transactionsService;

        public TransactionsController(ILogger<TransactionsController> logger, WalletDbContext db, TransactionsService transactionsService) : base(db)
        {
            this.logger = logger;
            this.transactionsService = transactionsService;
        }

        [HttpGet("{userId}/out")]
        [ProducesResponseType(typeof(Transaction[]), 200)]
        public async Task<Transaction[]> GetOutgoingTransactions(int userId)
        {
            logger.LogInformation($"Retrieving outgoing transactions for user id '{userId}'...");
            return await Db.Transactions
                .Where(x => x.ToUserId == userId)
                .OrderByDescending(x => x.TransactionAt)
                .ToArrayAsync();
        }

        [HttpGet("{userId}/in")]
        [ProducesResponseType(typeof(Transaction[]), 200)]
        public async Task<Transaction[]> GetIncomingTransactions(int userId)
        {
            logger.LogInformation($"Retrieving incoming transactions for user id '{userId}'...");
            return await Db.Transactions
                .Where(x => x.FromUserId == userId)
                .OrderByDescending(x => x.TransactionAt)
                .ToArrayAsync();
        }

        [HttpPost("pay")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(SerializableError), 400)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Pay([FromBody] PayRequest request)
        {
            logger.LogInformation($"Making a payment to user '{request.Email}' with the amount of '{request.Amount}' for the purpose of '{request.Purpose}'...");
            var toUser = await Db.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            if (toUser == null)
            {
                logger.LogInformation($"The email provided does not exist.");
                ModelState.AddModelError("Email", "The email provided does not exist.");
                return BadRequest(ModelState);
            }
            var transaction = new Transaction
            {
                FromUserId = UserId,
                ToUserId = toUser.UserId,
                Amount = request.Amount,
                TransactionAt = DateTime.Now,
                TransactionId = Guid.NewGuid(),
                Purpose = request.Purpose
            };
            transaction.IsSuspicious = await transactionsService.IsSuspiciousAsync(transaction);
            await Db.AddAsync(transaction);
            toUser.Balance += request.Amount;
            var fromUser = await UserQuery.SingleAsync();
            fromUser.Balance -= request.Amount;
            await Db.SaveChangesAsync();
            return Ok();
        }
    }
}
