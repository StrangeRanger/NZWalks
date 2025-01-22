// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace NZWalks.API.Models.Domain;

public class Difficulty
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}