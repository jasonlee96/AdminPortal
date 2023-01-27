using AdminPortal.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Business.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> CreateAsync(T entity, CommonTrailModel trail, Context outerContext = null);
        Task<T> UpdateAsync(T entity, CommonTrailModel trail, Context outerContext = null);
        Task<T> GetAsync(int key, Context outerContext = null);
        Task DeleteAsync(int key, CommonTrailModel trail, Context outerContext = null);

        AuditTrail GenerateAuditTrail(TrailAction action, int recordId, string oldJson, string newJson, CommonTrailModel trail);
    }
}
