using AutoMapper;
using CoronaClinic.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Member, MemberDto>();
        CreateMap<Immune, ImmuneDto>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id))
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Name));
    }
}
