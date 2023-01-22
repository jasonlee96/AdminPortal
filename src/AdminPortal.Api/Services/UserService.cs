using AdminPortal.Api.Repositories;
using AdminPortal.Api.Services.Interfaces;
using CommonService.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Claims;

namespace AdminPortal.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepo, ILogger<UserService> logger, IPasswordHasher<User> passwordHasher) 
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }
        public async Task<User> CreateUser(User user, CommonTrailModel trail)
        {

            return await _userRepo.CreateAsync(user, trail);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepo.GetAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string username, string email, string role, int pageIndex = 0, int pageSize = 20)
        {

            return await _userRepo.ListUserAsync(username, email, role, pageIndex, pageSize);
        }

        public async Task<UserAuthenticationResponse> SignIn(UserAuthenticationRequest request)
        {

            var user = await _userRepo.GetUserByEmail(request.Username);

            if (user == null) throw new Exception("User was not exist in the system");

            if (user.Status != Data.Enums.Status.Active) throw new Exception("User was not in active state");

            var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if(passwordCheck == PasswordVerificationResult.Success)
            {
                var response = new UserAuthenticationResponse()
                {
                    Email = user.Email,
                    Id = user.Id,
                    Username = user.UserName
                };

                response.Token = Jwt.GenerateToken(user.Id.ToString(), new Dictionary<string, string>()
                {
                    [ClaimTypes.Name] = user.UserName,
                    [ClaimTypes.Email] = user.Email,
                    ["Lang"] = "EN",
                    [ClaimTypes.Role] = string.Join(",", user.UserRoles.Select(x => x.Role.Code).ToArray())
                });

                return response;
            }
            else
            {
                throw new Exception("Incorrect password");
            }

            throw new Exception("Login failed");
        }
    }
}
