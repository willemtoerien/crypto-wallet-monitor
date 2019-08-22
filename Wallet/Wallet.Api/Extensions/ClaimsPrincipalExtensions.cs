using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace Wallet.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public const string Unauthenticated = "Principal's identity is not authenticated";
        public const string NoClaims = "The principal's identity has no name claims.";
        public const string InvalidClaimValue = "The claim's value of principal's identity is invalid.";

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (!principal.Identity.IsAuthenticated)
            {
                throw new AuthenticationException(Unauthenticated);
            }

            var identity = (ClaimsIdentity)principal.Identity;
            var claim = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new AuthenticationException(NoClaims);
            }

            if (int.TryParse(claim.Value, out var userId))
            {
                return userId;
            }
            throw new AuthenticationException(InvalidClaimValue);
        }
    }
}
