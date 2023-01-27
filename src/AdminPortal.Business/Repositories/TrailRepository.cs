using AdminPortal.Data;
using AdminPortal.Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;

namespace AdminPortal.Business.Repositories
{
    public class TrailRepository
    {
        private readonly IDbContextFactory<Context> _factory;
        public TrailRepository(IDbContextFactory<Context> factory) 
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public async Task CreateLog(AuditTrail data, string primaryKey = "Id", Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                await context.AddAsync(data);
                await context.SaveChangesAsync();
            }
            catch
            {
                
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }
    }
}
