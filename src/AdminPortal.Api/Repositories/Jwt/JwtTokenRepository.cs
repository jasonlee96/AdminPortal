using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Api.Repositories.Jwt
{
    public class JwtTokenRepository : IJwtTokenRepository
    {
        private readonly IDbContextFactory<Context> _factory;

        public JwtTokenRepository(IDbContextFactory<Context> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public async Task<JwtToken> CreateAsync(JwtToken entity, CommonTrailModel trail, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return new JwtToken();
            }
            catch
            {
                throw;
            }
            finally
            {
                if(outerContext == null) await context.DisposeAsync();
            }
        }

        public Task<JwtToken> DeleteAsync(int key, CommonTrailModel trail, Context outerContext = null)
        {
            throw new NotImplementedException();
        }

        public Task<JwtToken> GetAsync(int key, CommonTrailModel trail, Context outerContext = null)
        {
            throw new NotImplementedException();
        }

        public Task<JwtToken> UpdateAsync(JwtToken entity, CommonTrailModel trail, Context outerContext = null)
        {
            throw new NotImplementedException();
        }
    }
}
