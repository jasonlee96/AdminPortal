namespace AdminPortal.Business.Repositories
{
    public interface IJwtTokenRepository: IBaseRepository<JwtToken>
    {
        Task<JwtToken> GetJwtTokenAsync(string token, Context outerContext = null);
    }
}
