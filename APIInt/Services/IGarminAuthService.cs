namespace APIInt.Services
{
    public interface IGarminAuthService
    {
        Task<string> GetAuthorizationUrlAsync();
        Task ExchangeAccessTokenAsync(string oauthToken, string oauthVerifier);
    }
}
