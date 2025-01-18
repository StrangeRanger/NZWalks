using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Dto.Domain;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    // Represents a session with the database, allowing us to query and save instances of our entities.
    private readonly NZWalksDbContext _dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {
        // Get data from database.
        List<Region> regionsDomainModel = await _dbContext.Regions.ToListAsync();

        // Map Domain Models to DTOs.
        List<RegionDto> regionDtos = new();
        foreach (Region region in regionsDomainModel)
        {
            regionDtos.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }

        // Return DTOs.
        return Ok(regionDtos);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {
        // Get data from database.
        Region? regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

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
        Region regionDomainModel = new()
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        // Add region to database.
        await _dbContext.Regions.AddAsync(regionDomainModel);
        await _dbContext.SaveChangesAsync();

        // Map Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        // Return DTO.
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Get data from database.
        Region? region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Update region in database.
        region.Code = updateRegionRequestDto.Code;
        region.Name = updateRegionRequestDto.Name;
        region.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        // Save changes to database.
        await _dbContext.SaveChangesAsync();

        // Convert Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl
        };

        // Return DTO.
        return Ok(regionDto);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        // Get data from database.
        Region? region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Remove region from database.
        _dbContext.Regions.Remove(region);
        await _dbContext.SaveChangesAsync();

        // Convert Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl
        };

        // Return deleted region.
        return Ok(regionDto);
    }
}