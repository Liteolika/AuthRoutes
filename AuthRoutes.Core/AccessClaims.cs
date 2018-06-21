using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core
{
    public class AccessClaims
    {
        public static ClaimsBuilder SecureDataAccess { get; } = new ClaimsBuilder("SecureDataAccess");

        public static IEnumerable<Claim> GetAccessClaims()
        {
            foreach (var prop in new AccessClaims().GetType().GetProperties())
            {
                if (!(prop.GetValue(prop) is ClaimsBuilder claimsBuilderProp)) continue;
                yield return claimsBuilderProp.Readonly;
                yield return claimsBuilderProp.ReadWrite;
            }
        }

        public static IEnumerable<Claim> GetAccessClaimsWithValue(string claimValue)
        {
            foreach (var prop in new AccessClaims().GetType().GetProperties())
            {
                if (!(prop.GetValue(prop) is ClaimsBuilder claimsBuilderProp)) continue;
                yield return new Claim(claimsBuilderProp.ClaimType, claimValue);
            }
        }

        public static IEnumerable<string> GetAccessClaimTypes()
        {
            foreach (var prop in new AccessClaims().GetType().GetProperties())
            {
                if (prop.GetValue(prop) is ClaimsBuilder claimsBuilderProp)
                    yield return claimsBuilderProp.ClaimType;
            }
        }

    }
}
