using Microsoft.EntityFrameworkCore;

using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("NZWalksConnectionString");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // Required to use the controllers inside the 'Controllers' folder.

// Inject the DbContext into the services' container.
builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(connectionString));
// Inject the RegionRepository into the services' container. This will allow us to use the RegionRepository in the
// RegionsController.
builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();
// Inject the WalkRepository into the services' container. This will allow us to use the WalkRepository in the
// WalksController.
builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();
// Inject AutoMapper into the services' container. This will allow us to use AutoMapper in within the Controllers.
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Required to use the controllers inside the 'Controllers' folder.
app.Run();