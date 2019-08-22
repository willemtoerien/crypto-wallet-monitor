using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Wallet.Api.Areas.Users;
using Wallet.Api.Areas.Users.Services;

namespace Wallet.Api.Areas.Users.Services
{
    public class TokenProvider
    {
        private readonly UsersOptions options;
        private readonly ILogger<TokenProvider> logger;

        public TokenProvider(ILogger<TokenProvider> logger, IOptions<UsersOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        public string Provide(int userId)
        {
            logger.LogInformation($"Providing a token for id '{userId}'...");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(options.AuthenticationTokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);

            return tokenStr;
        }
    }
}
