using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SqlWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <param name="filterOn">The property/table column to filter on, such as 'Name'.</param>
    /// <param name="filterQuery">
    /// The text to filter for. For example, if 'filterOn' is 'Name', then 'filterQuery' could be 'beach' to find all
    /// entities with 'beach' in their name.
    /// </param>
    [SuppressMessage("Globalization", "CA1311:Specify a culture or use an invariant version")]
    [SuppressMessage("Globalization", "CA1304:Specify CultureInfo")]
    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
    {
        // Get all walks from the database, and make them queryable.
        // The 'Include' method is used to include related entities in the query results. Entity Framework knows what to
        // include based on the navigation properties defined in the model classes.
        IQueryable<Walk> walks = _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();

        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            switch (filterOn.ToLower())
            {
                case "name":
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                    break;
                case "description":
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                    break;
                default:
                    // TODO: Figure out a better way to inform the client that the filterOn value is invalid.
                    Console.WriteLine($"Invalid filterOn value: {filterOn}");
                    return new List<Walk>();
            }
        }

        return await walks.ToListAsync();
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