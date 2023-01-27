namespace AdminPortal.GraphQL.Mappers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() {
            CreateMap<User, UserType>().ReverseMap();
            CreateMap<UserRole, UserRoleType>().ReverseMap();
            CreateMap<Role, RoleType>().ReverseMap();
        }
    }
}
