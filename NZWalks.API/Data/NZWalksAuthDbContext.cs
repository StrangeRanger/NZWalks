using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

// ReSharper disable once InconsistentNaming
public class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        string readerRoleId = "8e7b1bd2-5779-4020-97ce-0eefcb9aa4c4";
        string writerRoleId = "c9daf664-e9e5-4bfb-a265-df96a4e61861";

        List<IdentityRole> roles = new()
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "READER"
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "WRITER"
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}