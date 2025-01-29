using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Dto.Auth;

public record class LoginResponseDto
{
    public string? JwtToken { get; init; }
    public required IdentityResult Result { get; init; }
}