namespace AdminPortal.Api.Repositories.Jwt
{
    public interface IJwtTokenRepository
    {
        Task<JwtToken> GetJwtTokenAsync(string token, Context outerContext = null);
    }
}
