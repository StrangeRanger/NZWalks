using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NZWalks.API.Data;
using NZWalks.API.Identity;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using NZWalks.API.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("NZWalksConnectionString") ??
                          throw new InvalidOperationException("Connection string is missing.");
string authConnectionString = builder.Configuration.GetConnectionString("NZWalksAuthConnectionString") ??
                              throw new InvalidOperationException("Auth connection string is missing.");
JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>() ??
                                    throw new InvalidOperationException("Jwt configuration is missing.");

// ------ Add services to the container. ------ //
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Add the API explorer service to the dependency injection container.
// This service is used to generate metadata for the API endpoints.
builder.Services.AddEndpointsApiExplorer();
// Add the Swagger generator to the dependency injection container.
// This service is used to generate Swagger documentation for the API.
builder.Services.AddSwaggerGen(options =>
{
    // Configure ability to use JWT Bearer authentication in Swagger UI.
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, 
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Add the MVC controllers to the dependency injection container.
// This service is used to enable the use of controllers in the application.
builder.Services.AddControllers();

// Register the NZWalksDbContext with the dependency injection container and configure it to use SQL Server with the
// provided connection string.
builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(connectionString));
// Register the NZWalksAuthDbContext with the dependency injection container and configure it to use SQL Server with the
// provided authentication connection string.
builder.Services.AddDbContext<NZWalksAuthDbContext>(options => options.UseSqlServer(authConnectionString));

// Register the IRegionRepository interface with the dependency injection container and configure it to use the
// SqlRegionRepository implementation.
builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();
// Register the IWalkRepository interface with the dependency injection container and configure it to use the
// SqlWalkRepository implementation.
builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();
// Register the AuthService class with the dependency injection container and configure it to use the scoped lifetime.
// This ensures that a new instance of AuthService is created for each request.
builder.Services.AddScoped<AuthService>();
// Register the TokenService class with the dependency injection container and configure it to use the scoped lifetime.
// This ensures that a new instance of TokenService is created for each request.
builder.Services.AddScoped<TokenService>();

// Register AutoMapper with the dependency injection container and configure it to use the specified AutoMapper
// profiles.
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// TODO: Add comments to explain the purpose of the following code and how it functions...
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

// Configure the identity system to require strong passwords for user accounts.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
});

// Register the JwtConfiguration instance as a singleton service in the dependency injection container.
// This ensures that the same instance of JwtConfiguration is used throughout the application.
builder.Services.AddSingleton(jwtConfiguration);
// Add the authentication services to the dependency injection container and configure it to use JWT Bearer
// authentication.
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

// If the application is running in the development environment, enable the Swagger middleware to serve the generated
// Swagger as a JSON endpoint and the Swagger UI middleware to serve the Swagger UI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Enable HTTPS redirection for the application.
app.UseAuthentication(); // Enable authentication middleware to validate and authenticate users.
app.UseAuthorization(); // Enable authorization middleware to enforce access control policies.
app.MapControllers(); // Map the controller routes to the endpoints.
app.Run(); // Run the application.