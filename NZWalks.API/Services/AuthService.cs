using Microsoft.AspNetCore.Identity;

using NZWalks.API.Dto.Auth;

namespace NZWalks.API.Services;

public class AuthService
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
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
}