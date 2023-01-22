using AdminPortal.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace AdminPortal.Api.Repositories
{
    public class UserRepository : TrailRepository, IUserRepository
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IDbContextFactory<Context> factory, ILogger<UserRepository> logger) : base(factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<User> CreateAsync(User entity, CommonTrailModel trail, Context outerContext = null)
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
                if (record == null) { throw new Exception("No record found"); }
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
                TableName = nameof(User),
                OldRecord = oldJson,
                NewRecord = newJson,
                RecordedAt = DateTime.UtcNow
            };
        }

        public async Task<User> GetAsync(int key, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.Users.Include(x => x.UserRoles)
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

        public async Task<Role> GetRoleByCode(string code, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.Roles
                             where x.Code == code
                             select x).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetRoleByCode));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public async Task<User> GetUserByEmail(string email, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                             where x.Email == email
                             select x).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetUserByEmail));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public async Task<IEnumerable<User>> ListUserAsync(string username, string email, string role, int pageIndex = 0, int pageSize = 20, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                Role roleModel = null;
                if(role != null)
                {
                    roleModel = await GetRoleByCode(role);

                    if (roleModel == null) throw new Exception("Role not found");
                }

                var query = (from x in context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                             where string.IsNullOrEmpty(username) ? true : x.UserName.Contains(username)
                             && string.IsNullOrEmpty(email) ? true : x.Email.Contains(email)
                             && role == null ? true : x.UserRoles.Any(x => x.Role.Code == roleModel.Code)
                             select x);

                var totalCount = await query.LongCountAsync();

                query = query.Skip(pageIndex * pageSize).Take(pageSize);

                var result = await query.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(ListUserAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public Task<IEnumerable<User>> ListUserAsync(string username, string email, UserRole role, int pageIndex = 0, int pageSize = 20, Context outerContext = null)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateAsync(User entity, CommonTrailModel trail, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                var record = await GetAsync(entity.Id, context);
                if (record == null) { throw new Exception("No record found"); }

                string intialRecord = JsonConvert.SerializeObject(record);


                //Update Logics
                if (entity.UserRoles != null)
                {
                    // remove all
                    context.RemoveRange(record.UserRoles);
                    // add current 
                    context.AddRange(entity.UserRoles);
                }
                if (entity.UserName != null) record.UserName = entity.UserName;
                if (entity.FirstName != null) record.FirstName = entity.FirstName;
                if (entity.LastName != null) record.LastName = entity.LastName;
                if (entity.PasswordHash != null) record.PasswordHash = entity.PasswordHash;
                if (entity.Status != null) record.Status = entity.Status;


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
