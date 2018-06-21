using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core.Services
{
    public interface ITokenService
    {
        Task<string> GetBearerToken();
    }

    public class TokenService : ITokenService
    {
        private TokenData _tokenData;
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;

        public TokenService(string url, string clientId, string clientSecret, string scope)
        {
            _url = url;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        public async Task<string> GetBearerToken()
        {
            if (_tokenData == null || _tokenData.Expires < DateTime.Now.AddSeconds(20))
            {
                var disco = await DiscoveryClient.GetAsync(_url);
                var tokenClient = new TokenClient(disco.TokenEndpoint, _clientId, _clientSecret);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(_scope);
                if (tokenResponse.IsError)
                    throw new UnauthorizedAccessException("Could not get access token.");

                _tokenData = new TokenData(tokenResponse.AccessToken, DateTime.Now.AddSeconds(tokenResponse.ExpiresIn));
            }

            if (_tokenData != null)
                return _tokenData.AccessToken;
            return "";

        }

        class TokenData
        {
            public TokenData(string accessToken, DateTime expires)
            {
                AccessToken = accessToken;
                Expires = expires;
            }

            public string AccessToken { get; }
            public DateTime Expires { get; }
        }

    }
}
