using AdminPortal.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminPortal.Api.Repositories
{
    public class JwtTokenRepository : TrailRepository, IJwtTokenRepository
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly ILogger<JwtTokenRepository> _logger;

        public JwtTokenRepository(IDbContextFactory<Context> factory) :base (factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<JwtToken> CreateAsync(JwtToken entity, CommonTrailModel trail, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();

                await CreateLog(GenerateAuditTrail(TrailAction.Insert, entity.Id, null, JsonConvert.SerializeObject(entity), trail));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(CreateAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }

            return entity;
        }

        public async Task DeleteAsync(int key, CommonTrailModel trail, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                var record = await GetAsync(key, context);
                if(record == null) { throw new Exception("No record found"); }
                context.Remove(record);
                await context.SaveChangesAsync();

                await CreateLog(GenerateAuditTrail(TrailAction.Delete, key, JsonConvert.SerializeObject(record), null, trail));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(DeleteAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public AuditTrail GenerateAuditTrail(TrailAction action, int recordId, string oldJson, string newJson, CommonTrailModel trail)
        {
            return new AuditTrail()
            {
                Action = action,
                IPAddress = trail.IPAddress,
                InitiatedBy = trail.TriggeredBy,
                Key = recordId,
                TableName = nameof(JwtToken),
                OldRecord = oldJson,
                NewRecord = newJson,
                RecordedAt = DateTime.UtcNow
            };
        }

        public async Task<JwtToken> GetAsync(int key, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.JwtTokens
                             where x.Id == key
                             select x).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public async Task<JwtToken> GetJwtTokenAsync(string token, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await (from x in context.JwtTokens
                              where x.Token == token
                              select x).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetJwtTokenAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public async Task<JwtToken> UpdateAsync(JwtToken entity, CommonTrailModel trail, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                var record = await GetAsync(entity.Id, context);
                if (record == null) { throw new Exception("No record found"); }

                string intialRecord = JsonConvert.SerializeObject(record);

                //Update Logics
                if(entity.Token != null)
                {
                    record.Token = entity.Token;
                }
                record.UserId = entity.UserId;
                record.IsValid = entity.IsValid;

                
                await context.SaveChangesAsync();

                await CreateLog(GenerateAuditTrail(TrailAction.Update, entity.Id, intialRecord, JsonConvert.SerializeObject(record), trail));

                return record;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(UpdateAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }
    }
}
