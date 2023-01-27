namespace AdminPortal.Business.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<Role> GetRoleByCode(string code, Context outerContext = null);
        Task<User> GetUserByEmail(string email, Context outerContext = null);
        Task<IEnumerable<User>> ListUserAsync(string username, string email, string role, int pageIndex = 0, int pageSize = 20, Context outerContext = null);
    }
}
