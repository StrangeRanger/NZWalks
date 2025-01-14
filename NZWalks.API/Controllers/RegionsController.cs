using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
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
        List<Region> regions = _dbContext.Regions.ToList();

        return Ok(regions);
    }

    [HttpGet("{id}")]
    public IActionResult GetRegionById(Guid id)
    {
        Region? region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

        if (region is null)
        {
            return NotFound();
        }

        return Ok(region);
    }
}