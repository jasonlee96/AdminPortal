using AdminPortal.Api.Services.Interfaces;

namespace AdminPortal.Api.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQuery 
    {
        public async Task<UserType> GetUser(
            [Service] IUserService service,
            [ScopedService] IMapper mapper,
            int id) => mapper.Map<UserType>(await service.GetUserById(id));

    }
}
