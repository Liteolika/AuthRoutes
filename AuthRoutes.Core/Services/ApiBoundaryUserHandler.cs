using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core.Services
{
    public static class ApiBoundaryUserHandler
    {
        public const string HeaderName = "requested-by";

        public static string GetUserName(this HttpRequest req)
        {
            return req.Headers[HeaderName];
        }
    }
}
