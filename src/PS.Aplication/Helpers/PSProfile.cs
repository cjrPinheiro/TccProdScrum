using AutoMapper;
using PS.Aplication.Dtos;
using PS.Application.Dtos;
using PS.Domain.Entities;
using PS.Domain.Entities.Identity;

namespace PS.Api.Business.Helpers
{
    public class PSProfile : Profile
    {
        public PSProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<User, UserExistingDto>()
                .ReverseMap();
            CreateMap<Project, ProjectDto>()
                .ReverseMap();
            CreateMap<TeamMember, TeamMemberDto>()
                .ReverseMap();
            CreateMap<JiraDomain, JiraDomainEditedDto>()
                .ReverseMap();
            CreateMap<JiraDomain, JiraDomainDto>()
              .ReverseMap();
            CreateMap<Sprint, SprintDto>()
               .ReverseMap();
            CreateMap<Status, StatusDto>()
               .ReverseMap();
        }
    }
}
