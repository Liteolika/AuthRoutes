using System;
using System.Collections.Generic;

namespace AuthRoutes.Core
{
    public class IdentityConfig
    {

        public class Scopes
        {
            public const string WebApplication = "webapplication";
            public const string WebApi = "webapi";
            public const string WebApiAdmin = "webapiAdmin";
        }

        public static class Resources
        {
            public const string WebApi = "webapi";
        }

        public static class Role
        {
            public const string PublicUser = "PublicUser";
            public const string AdminUser = "AdminUser";
        }

    }
}
