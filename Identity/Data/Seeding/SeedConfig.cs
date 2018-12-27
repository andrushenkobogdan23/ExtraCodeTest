using EnergySuite.SelfService.Identity.Models;
using IdentityServer4;
using IdentityServer4.Models;
using Shared.Enum;
using System.Collections.Generic;

namespace EnergySuite.SelfService.Identity.Data.Seeding
{
    public class SeedConfig
    {

        // scopes define the resources in your system
        internal static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        internal static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(IdentityScope.TodoRead, "Todo read API"),
                new ApiResource(IdentityScope.TodoCmd, "Todo cmd API"),
                new ApiResource(IdentityScope.TodoMessage, "Todo message API"),
            };
        }

        // clients want to access resources (aka scopes)
        internal static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "reader",
                    ClientName = "reader_client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(IdentityScope.Secret.Sha256())
                    },
                    AllowedScopes = { IdentityScope.TodoRead }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(IdentityScope.Secret.Sha256())
                    },
                    AllowedScopes = { IdentityScope.TodoRead, IdentityScope.TodoCmd }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = IdentityScope.TodoClient,
                    ClientName = IdentityScope.TodoClient,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false, // set true for consent page

                    ClientSecrets =
                    {
                        new Secret(IdentityScope.Secret.Sha256())
                    },

                    RedirectUris = { APIUrls.MVCAppSignin },
                    PostLogoutRedirectUris = { APIUrls.MVCAppSignout },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityScope.TodoRead,
                        IdentityScope.TodoCmd,
                        IdentityScope.TodoMessage,
                    },

                    AllowOfflineAccess = true
                }
            };
        }

        internal static IEnumerable<MyUser> GetUsers()
        {
            return new List<MyUser>
            {
                new MyUser
                {
                    Email = "rr@extracode.com.ua",
                    EmailConfirmed = true,
                    FirstName = "Roman",
                    LastName = "Romanchuk",
                    NormalizedEmail = "RR@EXTRACODE.COM.UA",
                    NormalizedUserName = "ROMAN",
                    UserName = "rr@extracode.com.ua",
                    PhoneNumber = "+380504465181",
                    PhoneNumberConfirmed = true,
                }
            };
        }
    }
}