using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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

        private readonly PasswordHasher hasher;
        private readonly Authenticator authenticator;

        public UsersController(WalletDbContext db, PasswordHasher hasher, Authenticator authenticator) : base(db)
        {
            this.hasher = hasher;
            this.authenticator = authenticator;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var isEmailUsed = await Db.Users.AnyAsync(x => x.Email == request.Email);
            if (isEmailUsed)
            {
                ModelState.AddModelError(nameof(SignUpRequest.Email), EmailInUse);
                return BadRequest(ModelState);
            }

            var user = new User();
            user.Email = request.Email;
            user.Password = hasher.Hash(request.Password);
            await Db.Users.AddAsync(user);

            await Db.SaveChangesAsync();

            authenticator.Authenticate(Response.Headers, user.UserId);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var user = await Db.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(SignInRequest.Email), InvalidEmail);
                return BadRequest(ModelState);
            }

            if (!hasher.Verify(user.Password, request.Password))
            {
                ModelState.AddModelError(nameof(SignInRequest.Email), InvalidPassword);
                return BadRequest(ModelState);
            }

            authenticator.Authenticate(Response.Headers, user.UserId);

            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserInfo), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get()
        {
            var user = await UserQuery.SingleOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("email/unique")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<bool> IsEmailUnique([FromQuery] string email)
        {
            var isEmailUsed = await Db.Users.AnyAsync(x => x.Email == email);
            return !isEmailUsed;
        }
    }
}
