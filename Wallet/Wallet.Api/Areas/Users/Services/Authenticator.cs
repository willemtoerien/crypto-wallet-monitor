using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Wallet.Api.Areas.Userss.Services
{
    public class Authenticator
    {
        private readonly TokenProvider tokenProvider;

        public Authenticator(TokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public void Authenticate(IDictionary<string, StringValues> headers, int userId)
        {
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
