using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSite.Ids4
{
    public static class OidcConfig
    {

        private static IConfiguration _config;


        public static void Configure(IConfiguration config)
        {
            _config = config;
        }

        //获取签发者地址，放置配置文件中主要用于当IssuerUri地址前置有代理节点时，两端一致
        public static string IssuerUri
        {
            get
            {
                string value = _config["IssuerUri"];
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new KeyNotFoundException("IssuerUri is not found");
                }
                if (string.IsNullOrWhiteSpace(value) || !Uri.TryCreate(value, UriKind.Absolute, out _))
                {
                    throw new NotSupportedException("Invalid IssuerUri ");
                }
                else
                {
                    return value;
                }
            }
        }

        //Oidc允许访问的客户端配置
        public static List<OidcClientConfig> OidcClients
        {
            get
            {
                var oidcClients = _config.GetSection("OidcClients").Get<List<OidcClientConfig>>();
                if (oidcClients == null)
                {
                    throw new KeyNotFoundException("OidcClients is not found");
                }

                return oidcClients;
            }
        }
    }

    public class OidcClientConfig
    {

        public string ClientId
        {
            get;
            set;
        }
        public string ClientName
        {
            get;
            set;
        }

        public string ClientWebHomepage
        {
            get;
            set;
        }

        public string OidcLoginCallback
        {
            get;
            set;
        }

        public string OidcFrontChannelLogoutCallback
        {
            get;
            set;
        }

        public string AllowedGrantTypes
        {
            get;
            set;
        }

        public bool RequireConsent
        {
            get;
            set;
        }

        public string[] AllowedScopes
        {
            get;
            set;
        }

        public bool FrontChannelLogoutSessionRequired
        {
            get;
            set;
        }

        public bool RequirePkce
        {
            get;
            set;
        }

        public string ClientSecrets
        {
            get;
            set;
        }
    }
}
