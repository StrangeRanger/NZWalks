using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using NZWalks.API.CustomActionFilters;
using NZWalks.API.Dto.Domain.Walk;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalksController : ControllerBase
{
    private readonly IRepository<Walk> _repository;
    private readonly IMapper _mapper;

    public WalksController(IRepository<Walk> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWalks()
    {
        // Get walks from database via Repository.
        List<Walk> walksDomainModel = await _repository.GetAllAsync();

        // Return DTOs.
        return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
    {
        // Get walk via id from database via Repository.
        Walk? walkDomainModel = await _repository.GetByIdAsync(id);

        if (walkDomainModel is null)
        {
            return NotFound();
        }

        // Return DTO.
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> AddWalks([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        // Map DTO to Domain Model.
        Walk walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

        // Add walk to database via Repository.
        Walk addedWalk = await _repository.AddAsync(walkDomainModel);

        // Return DTO.
        return CreatedAtAction(nameof(GetWalkById), new { id = addedWalk.Id }, _mapper.Map<AddWalkRequestDto>(addedWalk));
    }

    [HttpPut("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateWalks([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        // Map DTO to Domain Model.
        Walk walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

        // Update walk in database via Repository.
        Walk? updatedWalk = await _repository.UpdateAsync(id, walkDomainModel);

        if (updatedWalk is null)
        {
            return NotFound();
        }

        // Return DTO.
        return Ok(_mapper.Map<UpdateWalkRequestDto>(updatedWalk));
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteWalks([FromRoute] Guid id)
    {
        // Delete walk from database via Repository.
        Walk? deletedWalk = await _repository.DeleteAsync(id);

        if (deletedWalk is null)
        {
            return NotFound();
        }

        // Return DTO.
        return Ok(_mapper.Map<DeleteWalkRequestDto>(deletedWalk));
    }
}