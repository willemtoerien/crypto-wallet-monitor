﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Api.Areas.Transactions.Services;
using Wallet.Api.DataAccess;

namespace Wallet.Api.UnitTests.Areas.Transactions.Services
{
    [TestClass]
    public class TransactionsServiceTests
    {
        private WalletDbContext db;
        private TransactionsService service;

        [TestInitialize]
        public void Initialize()
        {
            var provider = new ServiceCollection()
                .AddLogging()
                .AddDbContext<WalletDbContext>(options => { options.UseInMemoryDatabase(Guid.NewGuid().ToString()); })
                .AddSingleton(sp =>
                {
                    var optionsMoq = new Mock<IOptions<TransactionsOptions>>();
                    optionsMoq.SetupGet(x => x.Value).Returns(new TransactionsOptions()
                    {
                        AverageAmountThresholdPercentage = .2M,
                        NonSuspiciousPurposes = new []{ "Fund", "Transfer", "Home", "Maintenance", "Bill", "Payments", "Salary", "Bonus", "Commission", "Purchase", "Inter", "Bank", "Transfer" },
                        PredefinedAmount = 100M
                    });
                    return optionsMoq.Object;
                })
                .AddTransient<TransactionsService>()
                .BuildServiceProvider();
            db = provider.GetService<WalletDbContext>();
            service = provider.GetService<TransactionsService>();
        }

        [TestMethod]
        public async Task IsAmountAboveThresholdAsyncEvaluatesCorrectly()
        {
            db.Transactions.AddRange(
                new Transaction { FromUserId = 0, TransactionId = Guid.NewGuid(), Amount = 50 },
                new Transaction { FromUserId = 0, TransactionId = Guid.NewGuid(), Amount = 50 },
                new Transaction { FromUserId = 0, TransactionId = Guid.NewGuid(), Amount = 50 }
            );
            db.SaveChanges();

            Assert.IsFalse(await service.IsAmountAboveThresholdAsync(0, 55M));
            Assert.IsFalse(await service.IsAmountAboveThresholdAsync(0, 45M));
            Assert.IsFalse(await service.IsAmountAboveThresholdAsync(0, 60M));
            Assert.IsTrue(await service.IsAmountAboveThresholdAsync(0, 61M));
        }
    }
}
