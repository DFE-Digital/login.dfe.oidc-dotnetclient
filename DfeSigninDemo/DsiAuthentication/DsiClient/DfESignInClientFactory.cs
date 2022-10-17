using System.Net.Http;

namespace DfeSigninDemo.DsiAuthentication.DsiClient
{
    public class DfESignInClientFactory
    {
        private readonly IDfESignInConfig _dfeSignInConfiguration;

        public DfESignInClientFactory(IDfESignInConfig dfeSignInConfiguration)
        {
            _dfeSignInConfiguration = dfeSignInConfiguration;
        }

        public DfESignInClient CreateDfESignInClient(string userId = "", string organizationId = "")
        {
            var dfeSignInClient = new DfESignInClient(new HttpClient());
            dfeSignInClient.ServiceId = _dfeSignInConfiguration.ClientId;
            dfeSignInClient.ServiceUrl = _dfeSignInConfiguration.APIServiceUrl;
            dfeSignInClient.UserId = userId;
            dfeSignInClient.OrganisationId = organizationId;

            var tokenData = new TokenData();
            tokenData.Header.Add(Constants.Header.Type, "JWT");
            var token = new TokenBuilder(new TokenDataSerializer(), tokenData, new TokenEncoder(), new JsonWebAlgorithm())
                .UseAlgorithm(_dfeSignInConfiguration.Cryptography)
                .ForAudience(_dfeSignInConfiguration.APIServiceAudience)
                .WithSecretKey(_dfeSignInConfiguration.APIServiceSecret)
                .Issuer(_dfeSignInConfiguration.ClientId)
                .CreateToken();

            dfeSignInClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return dfeSignInClient;
        }
    }
}
