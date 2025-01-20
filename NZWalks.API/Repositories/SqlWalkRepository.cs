using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository : IRepository<Walk>
{
    private readonly NZWalksDbContext _dbContext;

    public SqlWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Walk>> GetAllAsync()
    {
        // Get all walks from the database.
        // The 'Include' method is used to include related entities in the query results. Entity Framework knows what to
        // include based on the navigation properties defined in the model classes.
        return await _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        // Get the walk with the specified ID from the database.
        // The 'Include' method is used to include related entities in the query results. Entity Framework knows what to
        // include based on the navigation properties defined in the model classes.
        return await _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Walk> AddAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        // Find the existing walk in the database.
        Walk? existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);

        if (existingWalk is null)
        {
            return null;
        }

        // Update the existing walk with the new values.
        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.RegionId = walk.RegionId;
        existingWalk.DifficultyId = walk.DifficultyId;

        // Save the changes to the database and return the updated walk.
        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        // Find the existing walk in the database.
        Walk? existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);

        if (existingWalk is null)
        {
            return null;
        }

        // Remove the walk from the database and save the changes.
        _dbContext.Walks.Remove(existingWalk);
        await _dbContext.SaveChangesAsync();

        return existingWalk;
    }
}