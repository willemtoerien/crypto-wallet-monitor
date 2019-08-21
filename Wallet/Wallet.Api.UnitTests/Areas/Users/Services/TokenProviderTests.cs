using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Api.Areas.Userss.Services;

namespace Wallet.Api.UnitTests.Areas.Users.Services
{
    [TestClass]
    public class TokenProviderTests
    {
        private TokenProvider tokenProvider;

        [TestInitialize]
        public void Initialize()
        {
            tokenProvider = new ServiceCollection()
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
                .BuildServiceProvider()
                .GetService<TokenProvider>();
        }

        [TestMethod]
        public void TokenStringsAreProvidedCorrectly()
        {
            var token = tokenProvider.Provide(1);
            Assert.IsNotNull(token);
        }
    }
}
