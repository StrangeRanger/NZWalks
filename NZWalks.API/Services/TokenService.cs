using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using NZWalks.API.Identity;

namespace NZWalks.API.Services;

/*
 * Q: What are claims?
 * A: Claims are pieces of information about the user that can be used to make authorization decisions. For example, a
 *    claim could be the user's email address, or their role in the application.
 */
public class TokenService
{
    private readonly JwtConfiguration _jwtConfiguration;

    // Inject JwtConfiguration into the constructor.
    public TokenService(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        // ------ Claims ------ //

        // Create claims.
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        // Add roles to claims.
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // ------ Tokens ------ //

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtConfiguration.DurationInMinutes),
            signingCredentials: credentials
        );

        // ------ Return token ------ //

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}