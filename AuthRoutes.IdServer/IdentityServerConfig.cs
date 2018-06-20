using AuthRoutes.Core;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthRoutes.IdServer
{
    public class IdentityServerConfig
    {
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
                        Name = IdentityConfig.Scopes.WebApiApp,
                        DisplayName = "Access to Web API from a mobile client"
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
            //yield return new IdentityResource(IdentityConfig.Scopes.Backoffice, "Backoffice",
            //    AccessClaims.GetAccessClaimTypes());
        }

        public static IEnumerable<Client> GetClients()
        {
            yield return new Client()
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = new List<Secret> { new Secret("bikeXSecret".Sha256()) },
                AllowedScopes = { IdentityConfig.Scopes.WebApiApp },
                AccessTokenLifetime = 3600 * 24 * 7 //3600 = 1hour, * 24 = 1 day, * 7 = 1 week.
            };

            yield return new Client
            {
                ClientId = "webapi",
                ClientName = "Web API",
                ClientSecrets = { new Secret(clientsOptions.WebApi.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials
            };

            yield return new Client
            {
                ClientId = "backoffice",
                ClientName = "Backoffice",
                ClientSecrets = { new Secret(clientsOptions.Backoffice.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                RedirectUris = { $"{clientsOptions.Backoffice.Url}/signin-oidc" },
                PostLogoutRedirectUris = { $"{clientsOptions.Backoffice.Url}/signout-callback-oidc" },
                RequireConsent = false,
                AllowOfflineAccess = true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityConfig.Scopes.Backoffice,
                    IdentityConfig.Scopes.WebApiAdmin
                }
            };

            yield return new Client
            {
                ClientId = "backOfficeBackend",
                ClientName = "Backoffice Backend Client",
                ClientSecrets = { new Secret(clientsOptions.BackOfficeBackend.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string>
                {
                    IdentityConfig.Scopes.WebApiAdmin
                }
            };

            yield return new Client
            {
                ClientId = "swagger",
                ClientName = "Swagger UI Client Credentials",
                ClientSecrets = { new Secret("swagger".Sha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                RedirectUris = { "http://localhost:13404/swagger/o2c.html" },
                PostLogoutRedirectUris = { "http://localhost:13404/swagger/" },
                AllowedScopes = new List<string>
                {
                    IdentityConfig.Scopes.WebApiAdmin,
                    IdentityConfig.Scopes.WebApiApp
                }
            };

        }
    }
}
