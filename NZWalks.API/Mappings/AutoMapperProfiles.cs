using AutoMapper;

using NZWalks.API.Dto.Domain;
using NZWalks.API.Dto.Domain.Difficulty;
using NZWalks.API.Dto.Domain.Region;
using NZWalks.API.Dto.Domain.Walk;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // ------ Region mapping. ------ //

        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

        // ------ Walk mapping. ------ //

        CreateMap<Walk, WalkDto>().ReverseMap();
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();

        // ------ Difficulty mapping. ------ //

        // Mapping between Difficulty and DifficultyDto is needed because WalkDto includes a DifficultyDto property.
        // This ensures AutoMapper can map the Difficulty property of Walk to DifficultyDto in WalkDto.
        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
    }
}