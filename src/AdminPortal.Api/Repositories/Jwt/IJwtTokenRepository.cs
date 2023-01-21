namespace AdminPortal.Api.Repositories
{
    public interface IJwtTokenRepository
    {
        Task<JwtToken> GetJwtTokenAsync(string token, Context outerContext = null);
    }
}
