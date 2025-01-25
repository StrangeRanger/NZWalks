namespace NZWalks.API.Identity;

/*
 * This class is simply used to easily get and map the Jwt configuration from the 'appsettings.json' file, into an
 * object that can be used within the application. This makes the likelihood of accidentally misspelling a property
 * name, or using the wrong data type, much less likely.
 */
public class JwtConfiguration
{
    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int DurationInMinutes { get; init; }
}