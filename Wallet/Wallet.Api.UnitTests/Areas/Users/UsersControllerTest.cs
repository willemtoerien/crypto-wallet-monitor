using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Api.Areas.Users;
using Wallet.Api.Areas.Users.Requests;
using Wallet.Api.Areas.Users.Services;
using Wallet.Api.DataAccess;

namespace Wallet.Api.UnitTests.Areas.Users
{
    [TestClass]
    public class UsersControllerTest
    {
        private WalletDbContext db;
        private UsersController controller;

        [TestInitialize]
        public void Initialize()
        {
            var provider = new ServiceCollection()
                .AddDbContext<WalletDbContext>(options => { options.UseInMemoryDatabase(Guid.NewGuid().ToString()); })
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
                .AddTransient<PasswordHasher>()
                .AddTransient<Authenticator>()
                .AddTransient<UsersController>()
                .BuildServiceProvider();
            db = provider.GetService<WalletDbContext>();
            controller = provider.GetService<UsersController>();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [TestMethod]
        public async Task SignUpEmailInUse()
        {
            db.Users.Add(new User { Email = "test@test.com" });
            db.SaveChanges();
            var result = (BadRequestObjectResult)await controller.SignUp(new SignUpRequest
            {
                Email = "test@test.com"
            });
            var errors = (SerializableError)result.Value;
            Assert.AreEqual(UsersController.EmailInUse, ((string[])errors["Email"])[0]);
        }

        [TestMethod]
        public async Task SignUpSuccessful()
        {
            var request = new SignUpRequest
            {
                Email = "test@test.com",
                Password = "password"
            };
            var result = (OkObjectResult)await controller.SignUp(request);
            var user = (User)result.Value;
            Assert.AreEqual(request.Email, user.Email);

            var dbUser = db.Users.Single();
            Assert.AreEqual(request.Email, dbUser.Email);
        }

        [TestMethod]
        public async Task SignInInvalidEmail()
        {
            var result = (BadRequestObjectResult)await controller.SignIn(new SignInRequest
            {
                Email = "test@test.com"
            });
            var errors = (SerializableError)result.Value;
            Assert.AreEqual(UsersController.InvalidEmail, ((string[])errors["Email"])[0]);
        }

        [TestMethod]
        public async Task SignInInvalidPassword()
        {
            db.Users.Add(new User { Email = "test@test.com", Password = "Password" });
            db.SaveChanges();
            var result = (BadRequestObjectResult)await controller.SignIn(new SignInRequest
            {
                Email = "test@test.com",
                Password = "Invalid"
            });
            var errors = (SerializableError)result.Value;
            Assert.AreEqual(UsersController.InvalidPassword, ((string[])errors["Email"])[0]);
        }

        [TestMethod]
        public async Task SignInSuccessful()
        {
            db.Users.Add(new User { Email = "test@test.com", Password = new PasswordHasher().Hash("password") });
            db.SaveChanges();
            var request = new SignInRequest()
            {
                Email = "test@test.com",
                Password = "password"
            };
            var result = (OkObjectResult)await controller.SignIn(request);
            var user = (User)result.Value;
            Assert.AreEqual(request.Email, user.Email);
        }
    }
}
