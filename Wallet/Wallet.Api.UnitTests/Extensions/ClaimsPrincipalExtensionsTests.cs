using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wallet.Api.Extensions;

namespace Wallet.Api.UnitTests.Extensions
{
    [TestClass]
    public class ClaimsPrincipalExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUserIdThrowsArgumentNullException()
        {
            ClaimsPrincipalExtensions.GetUserId(null);
        }

        [TestMethod]
        public void GetUserIdThrowsAuthenticationExceptionForUnauthenticated()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var exception = Assert.ThrowsException<AuthenticationException>(() => principal.GetUserId());
            Assert.AreEqual(ClaimsPrincipalExtensions.Unauthenticated, exception.Message);
        }

        [TestMethod]
        public void GetUserIdThrowsAuthenticationExceptionForNoClaims()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "DummyAuthType"));
            var exception = Assert.ThrowsException<AuthenticationException>(() => principal.GetUserId());
            Assert.AreEqual(ClaimsPrincipalExtensions.NoClaims, exception.Message);
        }

        [TestMethod]
        public void GetUserIdThrowsAuthenticationExceptionForInvalidClaimValue()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new [] { new Claim(ClaimTypes.NameIdentifier, "abc"), }, "DummyAuthType"));
            var exception = Assert.ThrowsException<AuthenticationException>(() => principal.GetUserId());
            Assert.AreEqual(ClaimsPrincipalExtensions.InvalidClaimValue, exception.Message);
        }

        [TestMethod]
        public void GetUserIdIsRetrievedCorrectly()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new [] { new Claim(ClaimTypes.NameIdentifier, "1"), }, "DummyAuthType"));
            Assert.AreEqual(1, principal.GetUserId());
        }
    }
}
