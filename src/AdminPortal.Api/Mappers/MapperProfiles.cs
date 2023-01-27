namespace AdminPortal.Api.Mappers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() {
            CreateMap<User, UserType>().ReverseMap();
            CreateMap<Role, UserRoleType>().ReverseMap();
        }
    }
}
