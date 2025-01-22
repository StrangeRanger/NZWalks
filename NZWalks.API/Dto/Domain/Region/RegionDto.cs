namespace NZWalks.API.Dto.Domain.Region;

/*
 * We implement the DTOs, despite it feeling redundant, because it allows us to control what data is sent to the
 * client. This will allow flexibility in the future if we decide to change the structure of our domain models, and
 * adds a layer of security by not exposing all of our domain model's properties, and separating the concerns of our
 * domain models and the data we send to the client.
 */
public class RegionDto
{
    public Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? RegionImageUrl { get; init; }
}