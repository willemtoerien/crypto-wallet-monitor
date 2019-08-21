using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wallet.Api.Areas.Userss.Services;

namespace Wallet.Api.UnitTests.Areas.Users.Services
{
    [TestClass]
    public class PasswordHasherTests
    {
        [TestMethod]
        public void PasswordsAreHashedAndVerifiedCorrectly()
        {
            var hasher = new PasswordHasher();
            Assert.IsTrue(hasher.Verify(hasher.Hash("password"), "password"));
        }
    }
}
