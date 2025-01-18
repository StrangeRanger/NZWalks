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
        // Get Data From Database - Domain models.
        List<Region> regions = await _dbContext.Regions.ToListAsync();

        // Map Domain Models to DTOs.
        List<RegionDto> regionsDto = new();
        foreach (Region region in regions)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }

        // Return DTOs.
        return Ok(regionsDto);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {
        // Get Data From Database - Domain model.
        Region? region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Map Domain Model to DTO.
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

    [HttpPost]
    public async Task<IActionResult> AddRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // Map DTO to Domain Model.
        // TODO: Figure out how Id is getting created and retrieved in the 'regionDto'...
        Region region = new()
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        // Add Domain Model to Database.
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();

        // Map Domain Model to DTO.
        RegionDto regionDto = new()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl
        };

        // Return DTO.
        // TODO: Understand this portion better...
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }
    
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Check if region exists.
        Region? region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Update Domain Model.
        region.Code = updateRegionRequestDto.Code;
        region.Name = updateRegionRequestDto.Name;
        region.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        // Save Changes to Database.
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
        // Check if region exists.
        Region? region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Remove Domain Model from Database.
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