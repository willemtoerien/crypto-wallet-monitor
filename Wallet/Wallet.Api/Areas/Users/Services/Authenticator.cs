using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Wallet.Api.Areas.Userss.Services
{
    public class Authenticator
    {
        private readonly ILogger<Authenticator> logger;
        private readonly TokenProvider tokenProvider;

        public Authenticator(ILogger<Authenticator> logger, TokenProvider tokenProvider)
        {
            this.logger = logger;
            this.tokenProvider = tokenProvider;
        }

        public void Authenticate(IDictionary<string, StringValues> headers, int userId)
        {
            logger.LogInformation($"Authenticating user id '{userId}'...");
            var token = tokenProvider.Provide(userId);
            if (headers.ContainsKey("Authorization"))
            {
                return;
            }
            headers.Add("Access-Control-Expose-Headers", "Authorization");
            headers.Add("Authorization", $"Bearer {token}");
        }
    }
}
