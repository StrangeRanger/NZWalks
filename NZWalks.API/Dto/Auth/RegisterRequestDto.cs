using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Dto.Auth;

public class RegisterRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string[]? Roles { get; set; }

}