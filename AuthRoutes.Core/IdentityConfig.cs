using System;

namespace AuthRoutes.Core
{
    public class IdentityConfig
    {
        public static string IdentityServerUrl => "http://localhost:54597";

        public static class Resources
        {
            public static string WebApi => "webapi";
        }

        public static class Scopes
        {
            public static string WebApiAdmin => "";
            public static string WebApiApp => "";
            public static string Backoffice => "";
        }

    }
}
