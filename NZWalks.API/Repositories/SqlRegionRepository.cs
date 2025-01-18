using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SqlRegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Region>> GetAllRegionsAsync()
    {
        // Get all regions from the database.
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetRegionByIdAsync(Guid id)
    {
        // Get the region with the specified ID from the database.
        return await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Region> AddRegionAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
    {
        // Find the existing region in the database.
        Region? existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (existingRegion is null)
        {
            return null;
        }

        // Update the existing region with the new values.
        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.RegionImageUrl = region.RegionImageUrl;

        // Save the changes to the database and return the updated region.
        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region?> DeleteRegionAsync(Guid id)
    {
        // Find the existing region in the database.
        Region? existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (existingRegion is null)
        {
            return null;
        }

        // Remove the region from the database and save the changes.
        _dbContext.Regions.Remove(existingRegion);
        await _dbContext.SaveChangesAsync();

        return existingRegion;
    }
}