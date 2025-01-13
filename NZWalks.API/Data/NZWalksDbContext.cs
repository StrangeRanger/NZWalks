using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data;

/*
 * Q: What does DbContext do?
 * A: DbContext is the primary class that is responsible for interacting with the database. The DbContext acts as the
 *    bridge between your application and the database. It is responsible for managing the database connection,
 *    tracking changes to data, querying data, and saving data back to the database.
 */
// ReSharper disable once InconsistentNaming
public class NZWalksDbContext : DbContext
{
    /// Represents a collection of entities (Walk objects) that can be queried from the database. In this case, these
    /// entities are the Domain Models.
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Walk> Walks { get; set; }

    public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
    { }
}