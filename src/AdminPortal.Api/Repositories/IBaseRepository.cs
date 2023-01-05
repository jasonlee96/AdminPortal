using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Api.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> CreateAsync(T entity, CommonTrailModel trail, Context outerContext = null);
        Task<T> UpdateAsync(T entity, CommonTrailModel trail, Context outerContext = null);
        Task<T> GetAsync(int key, CommonTrailModel trail, Context outerContext = null);
        Task<T> DeleteAsync(int key, CommonTrailModel trail, Context outerContext = null);
    }
}
