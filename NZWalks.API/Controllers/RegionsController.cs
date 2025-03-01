using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NZWalks.API.CustomActionFilters;
using NZWalks.API.Dto.Domain.Region;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

// https://localhost:1234/api/regions
[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    // Represents a collection of methods that allow us to interact with the Region table in the database.
    private readonly IRegionRepository _repository;
    // Represents a type used to perform object-object mapping.
    private readonly IMapper _mapper;

    public RegionsController(IRegionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAllRegions()
    {
        // Get regions from database via Repository.
        List<Region> regionsDomainModel = await _repository.GetAllAsync();

        // Return DTOs.
        return Ok(_mapper.Map<List<RegionDto>>(regionsDomainModel));
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Roles = "Reader")]
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
    [ValidateModel]
    [Authorize(Roles = "Writer")]
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
    [ValidateModel]
    [Authorize(Roles = "Writer")]
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
    [Authorize(Roles = "Writer")]
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