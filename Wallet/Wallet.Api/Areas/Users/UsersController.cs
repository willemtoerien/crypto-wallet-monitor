using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtualAssessors.Api.Areas.Users.Requests;
using Wallet.Api.Areas.Users.Models;
using Wallet.Api.Areas.Users.Services;
using Wallet.Api.DataAccess;
using Wallet.Api.Areas.Userss.Services;

namespace Wallet.Api.Areas.Users
{
    [Route("users")]
    public class UsersController : Controller
    {
        public const string EmailInUse = "The email is already in use.";
        public const string InvalidEmail = "The email provided does not exist.";
        public const string InvalidPassword = "The password provided is incorrect.";

        private readonly ILogger<UsersController> logger;
        private readonly PasswordHasher hasher;
        private readonly Authenticator authenticator;

        public UsersController(ILogger<UsersController> logger, WalletDbContext db, PasswordHasher hasher, Authenticator authenticator) : base(db)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.authenticator = authenticator;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(SerializableError), 400)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            logger.LogInformation($"Signing up a new user with email '{request.Email}'...");
            try
            {
                var isEmailUsed = await Db.Users.AnyAsync(x => x.Email == request.Email);
                if (isEmailUsed)
                {
                    logger.LogInformation($"The email '{request.Email}' is already in use.");
                    ModelState.AddModelError(nameof(SignUpRequest.Email), EmailInUse);
                    return BadRequest(ModelState);
                }

                logger.LogDebug("Creating a new user...");
                var user = new User();
                user.Email = request.Email;
                user.Password = hasher.Hash(request.Password);
                user.Balance = 1000;
                await Db.Users.AddAsync(user);

                logger.LogDebug("Saving the data changes...");
                await Db.SaveChangesAsync();

                authenticator.Authenticate(Response.Headers, user.UserId);

                return Ok(user);
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(SerializableError), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            logger.LogInformation($"Signing ing a user with email '{request.Email}'...");
            try
            {
                var user = await Db.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
                if (user == null)
                {
                    logger.LogInformation($"The email '{request.Email}' does not exist.");
                    ModelState.AddModelError(nameof(SignInRequest.Email), InvalidEmail);
                    return BadRequest(ModelState);
                }

                if (!hasher.Verify(user.Password, request.Password))
                {
                    logger.LogInformation($"The password '{request.Password}' is incorrect.");
                    ModelState.AddModelError(nameof(SignInRequest.Email), InvalidPassword);
                    return BadRequest(ModelState);
                }

                authenticator.Authenticate(Response.Headers, user.UserId);

                return Ok(user);
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Retrieving the signed in user.");
            try
            {
                var user = await UserQuery.SingleOrDefaultAsync();
                if (user == null)
                {
                    return BadRequest("User not found.");
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }

        [AllowAnonymous]
        [HttpGet("email/unique")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> IsEmailUnique([FromQuery] string email)
        {
            logger.LogInformation($"Checking if '{email}' is unique...");
            try
            {
                var isEmailUsed = await Db.Users.AnyAsync(x => x.Email == email);
                return Ok(!isEmailUsed);
            }
            catch (Exception e)
            {
                logger.LogError("An unhandled exception was thrown.", e);
                return StatusCode(500, "Unexpected Server Error occurred.");
            }
        }
    }
}
