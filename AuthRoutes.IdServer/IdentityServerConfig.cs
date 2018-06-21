using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthRoutes.Core;
using IdentityServer4;
using IdentityServer4.Models;

namespace AuthRoutes.IdServer
{
    public class IdentityServerConfig
    {

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
            yield return new IdentityResource(
                IdentityConfig.Scopes.WebApplication,
                "DisplayName: WebApplication",
                AccessClaims.GetAccessClaimTypes());
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource
            {
                Name = IdentityConfig.Resources.WebApi,
                DisplayName = "Web API",
                Scopes =
                {
                    new Scope
                    {
                        Name = IdentityConfig.Scopes.WebApiAdmin,
                        DisplayName = "Full access to Web API"
                    },

                    new Scope
                    {
                        Name = IdentityConfig.Scopes.WebApi,
                        DisplayName = "Access to Web API from a mobile client"
                    }
                }
            };

            yield return new ApiResource
            {
                Name = IdentityConfig.Resources.IdentityServerApi,
                DisplayName = "Identity server api",
                Scopes =
                {
                    new Scope
                    {
                        Name = IdentityConfig.Scopes.IdServer,
                        DisplayName = "Access to id server API"
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {

            yield return new Client
            {
                ClientId = "webapplication_backend",
                ClientName = "WebApplication Backend",
                ClientSecrets = { new Secret("webapplication_backend".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string>
                {
                    IdentityConfig.Scopes.WebApi
                }
            };

            yield return new Client
            {
                ClientId = "webapi_backend",
                ClientName = "WebAPI Backend",
                ClientSecrets = { new Secret("webapi_backend".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string>
                {
                    IdentityConfig.Scopes.IdServer
                }
            };

            yield return new Client
            {
                ClientId = "webapplication",
                ClientName = "WebApplication",
                ClientSecrets = { new Secret("webapplication".Sha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                RedirectUris = { $"{SystemConfig.WebApplicationUrl}/signin-oidc" },
                PostLogoutRedirectUris = { $"{SystemConfig.WebApplicationUrl}/signout-callback-oidc" },
                RequireConsent = false,
                AllowOfflineAccess = true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityConfig.Scopes.WebApplication,
                    IdentityConfig.Scopes.WebApiAdmin
                }
            };

        }
    }
}
