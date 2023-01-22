namespace AdminPortal.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(string username, string email, string role, int pageIndex = 0, int pageSize = 20);

        Task<User> GetUserById(int id);

        Task<User> CreateUser(User user, CommonTrailModel trail);

        Task<UserAuthenticationResponse> SignIn(UserAuthenticationRequest request);

    }
}
