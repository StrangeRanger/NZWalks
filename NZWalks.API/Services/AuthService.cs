using Microsoft.AspNetCore.Identity;

using NZWalks.API.Dto.Auth;

namespace NZWalks.API.Services;

public class AuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        IdentityUser identityUser = new()
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        IdentityResult identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            if (registerRequestDto.Roles is not null && registerRequestDto.Roles.Length != 0)
            {
                try
                {
                    await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                }
                catch (Exception exception)
                {
                    identityResult = IdentityResult.Failed(new IdentityError
                    {
                        Code = "AddRolesFailed",
                        Description = exception.Message
                    });

                    Console.WriteLine("[INFO] Removing user from the database...");
                    await _userManager.DeleteAsync(identityUser);
                }
            }
        }
        else
        {
            Console.WriteLine("[ERROR] Something went wrong while creating the user.");
            Console.WriteLine("ERROR: " + identityResult.Errors.First().Description);
        }

        return identityResult;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        IdentityUser? user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user is not null)
        {
            bool checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (checkPasswordResult)
            {
                // Get the roles for the user.
                IList<string> roles = await _userManager.GetRolesAsync(user);

                if (roles is not null)
                {
                    // Create a JWT token.
                    string jwtToken = _tokenService.CreateJwtToken(user, roles.ToList());

                    // Return the token.
                    return new LoginResponseDto()
                    {
                        JwtToken = jwtToken,
                        Result = IdentityResult.Success
                    };
                }
            }
        }

        return new LoginResponseDto()
        {
            Result = IdentityResult.Failed(new IdentityError
            {
                Code = "LoginFailed",
                Description = "Invalid username or password."
            })
        };
    }
}