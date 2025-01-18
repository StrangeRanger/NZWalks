using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Dto.Domain;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    // Represents a collection of methods that allow us to interact with the Region table in the database.
    private readonly IRepository<Region> _repository;
    // Represents a type used to perform object-object mapping.
    private readonly IMapper _mapper;

    public RegionsController(IRepository<Region> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {
        // Get regions from database via Repository.
        List<Region> regionsDomainModel = await _repository.GetAllAsync();

        // Return DTOs.
        return Ok(_mapper.Map<List<RegionDto>>(regionsDomainModel));
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {
        // Get region via id from database via Repository.
        Region? regionDomainModel = await _repository.GetByIdAsync(id);

        if (regionDomainModel is null)
        {
            return NotFound();
        }

        // Return DTO.
        return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }

    [HttpPost]
    public async Task<IActionResult> AddRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // Map DTO to Domain Model.
        Region regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

        // Add region to database via Repository.
        await _repository.AddAsync(regionDomainModel);

        // Map Domain Model to DTO.
        RegionDto regionDto = _mapper.Map<RegionDto>(regionDomainModel);

        // Return DTO.
        // TODO: Understand this portion better...
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Map DTO to Domain Model.
        Region? regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

        // Update region in database via Repository.
        regionDomainModel = await _repository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel is null)
        {
            return NotFound();
        }

        // Return DTO.
        return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        // Delete region from database via Repository.
        Region? regionDomainModel = await _repository.DeleteAsync(id);

        if (regionDomainModel is null)
        {
            return NotFound();
        }
        
        // Return deleted region.
        return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }
}