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
        return await _dbContext.Walks.ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        // Get the walk with the specified ID from the database.
        return await _dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Walk> AddAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        throw new NotImplementedException();
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}