using AdminPortal.Api.Services.Interfaces;
using AdminPortal.Data.Enums;

namespace AdminPortal.Api.GraphQL.Types
{
        
    public class UserType 
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Status Status { get; set; }

        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }

        public IEnumerable<UserRoleType> UserRoles { get; set; }
    }

    public class UserRoleType
    {
        public int Id { get; set; }

        public string Role { get; set; }
    }
}
