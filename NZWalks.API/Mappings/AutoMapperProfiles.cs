using AutoMapper;

using NZWalks.API.Dto.Domain;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
    }
}