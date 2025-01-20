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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ------ Seed data for Difficulties: ------ //
        // ------   Easy, Medium, Hard        ------ //

        // Guids are hardcoded, because different Guids would be generated each time the application is run.
        List<Difficulty> difficulties = new()
        {
            new Difficulty { Id = Guid.Parse("f0db9573-0637-4272-880b-6e1415f80395"), Name = "Easy" },
            new Difficulty { Id = Guid.Parse("d988f4dc-86cf-4be4-a469-0080abcd6864"), Name = "Medium" },
            new Difficulty { Id = Guid.Parse("cf3f47c9-a345-4e2f-8ccf-3ae1c10ac418"), Name = "Hard" }
        };

        // Add the difficulty seed data to the database:
        //  - `Entity<Difficulty>()` is used to specify the entity type that the seed data belongs to.
        //  - `HasData(difficulties)` is used to specify the seed data that should be added to the database.
        modelBuilder.Entity<Difficulty>().HasData(difficulties);

        // ------ Seed data for Regions: ------ //

        // Guids are hardcoded, because different Guids would be generated each time the application is run.
        List<Region> regions = new()
        {
            new Region
            {
                Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                Name = "Auckland",
                Code = "AKL",
                RegionImageUrl = 
                    "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                Name = "Northland",
                Code = "NTL",
                RegionImageUrl = null
            },
            new Region
            {
                Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                Name = "Bay Of Plenty",
                Code = "BOP",
                RegionImageUrl = null
            },
            new Region
            {
                Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                Name = "Wellington",
                Code = "WGN",
                RegionImageUrl = 
                    "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                Name = "Nelson",
                Code = "NSN",
                RegionImageUrl = 
                    "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                Name = "Southland",
                Code = "STL",
                RegionImageUrl = null
            }
        };

        // Add the region seed data to the database.
        modelBuilder.Entity<Region>().HasData(regions);
    }
}