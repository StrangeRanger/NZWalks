using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // Required to use the controllers inside the 'Controllers' folder.

// Inject the DbContext into the services' container.
builder.Services.AddDbContext<NZWalksDbContext>(options =>
                                                {
                                                    // Specify the connection string used by the DbContext.
                                                    // NOTE: Connection string is specified within the
                                                    // 'appsettings.json' file.
                                                    options.UseSqlServer(
                                                        builder.Configuration.GetConnectionString("NZWalksDbContext"));
                                                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Required to use the controllers inside the 'Controllers' folder.
app.Run();
