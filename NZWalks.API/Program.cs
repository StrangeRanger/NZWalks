using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using NZWalks.API.Data;
using NZWalks.API.Identity;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("NZWalksConnectionString") ??
                          throw new InvalidOperationException("Connection string is missing.");
JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>() ??
                                    throw new InvalidOperationException("Jwt configuration is missing.");

// ------ Add services to the container. ------ //

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // TODO: Add comment about the purpose of this...
builder.Services.AddSwaggerGen(); // TODO: Add comment about the purpose of this...
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
// Inject authentication configurations into the services' container. This will allow us to use JWT authentication
// within the application.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfiguration.Issuer,
        ValidAudience = jwtConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey))
    });

// ------ Build the application and register the middleware. ------ //

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.
app.UseAuthentication(); // TODO: Add comment about the purpose of this...
app.UseAuthorization(); // TODO: Add comment about the purpose of this...
app.MapControllers(); // Required to use the controllers inside the 'Controllers' folder.
app.Run(); // Start the application.