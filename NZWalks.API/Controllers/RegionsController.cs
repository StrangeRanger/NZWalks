using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Dto.Domain;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    // Represents a collection of methods that allow us to interact with the Region table in the database.
    private readonly IRegionRepository _regionRepository;

    public RegionsController(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {
        // Get regions from database via Repository.
        List<Region> regionsDomainModel = await _regionRepository.GetAllRegionsAsync();

        // Map Domain Models to DTOs.
        List<RegionDto> regionDto = new();
        foreach (Region region in regionsDomainModel)
        {
            regionDto.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }

        // Return DTOs.
        return Ok(regionDto);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {
        // Get region via id from database via Repository.
        Region? regionDomainModel = await _regionRepository.GetRegionByIdAsync(id);

        if (regionDomainModel is null)
        {
            return NotFound();
        }

        // Map Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        // Return DTO.
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // Map DTO to Domain Model.
        // TODO: Figure out how Id is getting created and retrieved in the 'regionDto'...
        Region regionDomainModel = new()
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        // Add region to database via Repository.
        await _regionRepository.AddRegionAsync(regionDomainModel);

        // Map Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        // Return DTO.
        // TODO: Understand this portion better...
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Map DTO to Domain Model.
        Region? regionDomainModel = new()
        {
            Code = updateRegionRequestDto.Code,
            Name = updateRegionRequestDto.Name,
            RegionImageUrl = updateRegionRequestDto.RegionImageUrl
        };

        // Update region in database via Repository.
        regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

        if (regionDomainModel is null)
        {
            return NotFound();
        }

        // Convert Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        // Return DTO.
        return Ok(regionDto);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        // Delete region from database via Repository.
        Region? regionDomainModel = await _regionRepository.DeleteRegionAsync(id);

        if (regionDomainModel is null)
        {
            return NotFound();
        }

        // Convert Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        // Return deleted region.
        return Ok(regionDto);
    }
}