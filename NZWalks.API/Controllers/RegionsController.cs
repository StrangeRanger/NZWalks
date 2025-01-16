using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetAllRegions()
    {
        // Get Data From Database - Domain models.
        List<Region> regions = _dbContext.Regions.ToList();

        // Map Domain Models to DTOs.
        List<RegionDto> regionsDto = new();
        foreach (Region region in regions)
        {
            regionsDto.Add(new RegionDto() { Id = region.Id, Code = region.Code, Name = region.Name,
                                             RegionImageUrl = region.RegionImageUrl });
        }

        // Return DTOs.
        return Ok(regionsDto);
    }

    [HttpGet("{id}")]
    public IActionResult GetRegionById(Guid id)
    {
        // Get Data From Database - Domain model.
        Region? region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        // Map Domain Model to DTO.
        RegionDto regionDto =
            new() { Id = region.Id, Code = region.Code, Name = region.Name, RegionImageUrl = region.RegionImageUrl };

        // Return DTO.
        return Ok(regionDto);
    }
}