using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core
{
    public class ClaimsBuilder
    {
        public ClaimsBuilder(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }

        public Claim ReadWrite => new Claim(ClaimType, AccessClaimValues.ReadWrite);

        public Claim Readonly => new Claim(ClaimType, AccessClaimValues.Readonly);
    }
}
