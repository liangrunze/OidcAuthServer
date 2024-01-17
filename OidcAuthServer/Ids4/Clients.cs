using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;

namespace ServerSite.Ids4
{
    public static class Clients
    {


        public static IEnumerable<Client> All => GetClients();


        public static Client[] GetClients()
        {
            var oidcClients = OidcConfig.OidcClients.Select(x => new Client
            {
                ClientId = x.ClientId,
                ClientName = x.ClientName,
                PostLogoutRedirectUris = { x.ClientWebHomepage },
                RedirectUris = { x.ClientWebHomepage + x.OidcLoginCallback },
                AllowedGrantTypes = { x.AllowedGrantTypes },
                RequireConsent = x.RequireConsent,
                FrontChannelLogoutUri = x.ClientWebHomepage + x.OidcFrontChannelLogoutCallback,
                FrontChannelLogoutSessionRequired = x.FrontChannelLogoutSessionRequired,
                RequirePkce = x.RequirePkce,
                ClientSecrets = new List<Secret>
                    {
                        new Secret(x.ClientSecrets.Sha256())
                    },
                AllowedScopes = x.AllowedScopes
            }).ToArray();

            return oidcClients;
        }
    }
}
