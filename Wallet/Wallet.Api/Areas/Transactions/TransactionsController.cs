using System;
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
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetOutgoingTransactions(int userId)
        {
            logger.LogInformation($"Retrieving outgoing transactions for user id '{userId}'...");
            try
            {
                return Ok(await Db.Transactions
                    .Where(x => x.ToUserId == userId)
                    .OrderByDescending(x => x.TransactionAt)
                    .ToArrayAsync());
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }

        [HttpGet("{userId}/in")]
        [ProducesResponseType(typeof(Transaction[]), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetIncomingTransactions(int userId)
        {
            logger.LogInformation($"Retrieving incoming transactions for user id '{userId}'...");
            try
            {
                return Ok(await Db.Transactions
                    .Where(x => x.FromUserId == userId)
                    .OrderByDescending(x => x.TransactionAt)
                    .ToArrayAsync());
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }

        [HttpPost("pay")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(SerializableError), 400)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Pay([FromBody] PayRequest request)
        {
            logger.LogInformation($"Making a payment to user '{request.Email}' with the amount of '{request.Amount}' for the purpose of '{request.Purpose}'...");
            try
            {
                var toUser = await Db.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
                if (toUser == null)
                {
                    logger.LogInformation("The email provided does not exist.");
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
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }
    }
}
