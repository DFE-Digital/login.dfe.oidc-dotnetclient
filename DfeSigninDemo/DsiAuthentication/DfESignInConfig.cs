using System.Collections.Generic;

namespace DfeSigninDemo.DsiAuthentication
{
    public class DfESignInConfig : IDfESignInConfig
    {
        public string APIServiceAudience { get; set; }
        public string APIServiceProxyUrl { get; set; }
        public string APIServiceSecret { get; set; }
        public string APIServiceUrl { get; set; }
        public string Authority { get; set; }
        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int CookieExpiration { get; set; }
        public string CookieName { get; set; }
        public string Cryptography { get; set; }
        public bool GetClaimsFromUserInfoEndpoint { get; set; }
        public string MetaDataUrl { get; set; }
        public bool SaveTokens { get; set; }
        public List<string> Scopes { get; set; }
        public string SignoutCallbackUrl { get; set; }
        public string SignoutRedirectUrl { get; set; }
        public bool SlidingExpiration { get; set; }
        public bool UseDfeSignin { get; set; }
    }
}
