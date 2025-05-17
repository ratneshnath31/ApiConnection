using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using System.Web;

namespace APIInt.Services
{
    public class GarminAuthService : IGarminAuthService
    {
        private readonly GarminOptions _opts;

        public GarminAuthService(IOptions<GarminOptions> opts)
        {
            _opts = opts.Value;
        }

        public async Task<string> GetAuthorizationUrlAsync()
        {
            var client = new RestClient(new RestClientOptions(_opts.RequestTokenUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(
                    _opts.ConsumerKey,
                    _opts.ConsumerSecret,
                    _opts.CallbackUrl)
            });
            var resp = await client.ExecuteAsync(new RestRequest());
            var qs   = HttpUtility.ParseQueryString(resp.Content);
            return $"{_opts.AuthorizeUrl}?oauth_token={qs["oauth_token"]}";
        }

        public async Task ExchangeAccessTokenAsync(string oauthToken, string oauthVerifier)
        {
            var client = new RestClient(new RestClientOptions(_opts.AccessTokenUrl)
            {
                Authenticator = OAuth1Authenticator.ForAccessToken(
                    _opts.ConsumerKey,
                    _opts.ConsumerSecret,
                    oauthToken,
                    oauthVerifier)
            });
            var resp = await client.ExecuteAsync(new RestRequest());
            var qs   = HttpUtility.ParseQueryString(resp.Content);

            // persist these somewhere safe:
            var accessToken       = qs["oauth_token"];
            var accessTokenSecret = qs["oauth_token_secret"];
            // … your storage logic here …
        }
    }
}
