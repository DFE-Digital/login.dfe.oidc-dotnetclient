using System.Collections.Generic;

namespace DfeSigninDemo.DsiAuthentication
{
    public interface IDfESignInConfig
    {
        /// <summary>
        /// Typically "signin.education.gov.uk". Please Refer GitHub Documentation https://github.com/DFE-Digital/login.dfe.public-api#get-user-access-to-service
        /// </summary>
        string APIServiceAudience { get; set; }
        /// <summary>
        /// Absolute URL Path. Set ONLY if Required By Middleware
        /// </summary>
        string APIServiceProxyUrl { get; set; }
        string APIServiceSecret { get; set; }
        /// <summary>
        /// Absolute URL Path. Environment Specifc 
        /// </summary>
        string APIServiceUrl { get; set; }
        string Authority { get; set; }
        string CallbackUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        int CookieExpiration { get; set; }
        string CookieName { get; set; }
        /// <summary>
        /// Currently only supports HMAC types
        /// </summary>
        string Cryptography { get; set; }
        bool GetClaimsFromUserInfoEndpoint { get; set; }
        string MetaDataUrl { get; set; }
        bool SaveTokens { get; set; }
        List<string> Scopes { get; set; }
        string SignoutCallbackUrl { get; set; }
        string SignoutRedirectUrl { get; set; }
        bool SlidingExpiration { get; set; }
        bool UseDfeSignin { get; set; }
    }
}
