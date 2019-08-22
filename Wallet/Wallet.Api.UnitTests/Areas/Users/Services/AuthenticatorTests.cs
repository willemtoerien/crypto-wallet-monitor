using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Api.Areas.Users.Services;

namespace Wallet.Api.UnitTests.Areas.Users.Services
{
    [TestClass]
    public class AuthenticatorTests
    {
        private Authenticator authenticator;

        [TestInitialize]
        public void Initialize()
        {
            authenticator = new ServiceCollection()
                .AddLogging()
                .AddSingleton(sp =>
                {
                    var optionsMoq = new Mock<IOptions<UsersOptions>>();
                    optionsMoq.SetupGet(x => x.Value).Returns(new UsersOptions()
                    {
                        AuthenticationTokenKey = Guid.Empty.ToString()
                    });
                    return optionsMoq.Object;
                })
                .AddTransient<TokenProvider>()
                .AddTransient<Authenticator>()
                .BuildServiceProvider()
                .GetService<Authenticator>();
        }

        [TestMethod]
        public void AuthenticateDoesNotDuplicate()
        {
            var headers = new Dictionary<string, StringValues>()
            {
                {"Authorization", "Nonsense"}
            };
            authenticator.Authenticate(headers, 1);
        }

        [TestMethod]
        public void AuthenticateAddHeadersCorrectly()
        {
            var headers = new Dictionary<string, StringValues>();
            authenticator.Authenticate(headers, 1);
            Assert.AreEqual(new StringValues("Authorization"), headers["Access-Control-Expose-Headers"]);
            Assert.IsTrue(((string)headers["Authorization"]).StartsWith("Bearer"));
        }
    }
}
