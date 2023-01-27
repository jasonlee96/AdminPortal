using AdminPortal.Business.Services.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace AdminPortal.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQuery 
    {
        public async Task<UserType> GetUser(
            [Service] IUserService service,
            [Service] IMapper mapper,
            int id)
        {
            var user = await service.GetUserById(id);
            return mapper.Map<UserType>(user);
        }

    }
}
