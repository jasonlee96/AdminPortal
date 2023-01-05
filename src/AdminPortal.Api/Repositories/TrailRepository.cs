using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AdminPortal.Api.Repositories
{
    public class TrailRepository
    {
        private readonly IDbContextFactory<Context> _factory;
        public TrailRepository(IDbContextFactory<Context> factory) 
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        //public async Task CreateLog
    }
}
