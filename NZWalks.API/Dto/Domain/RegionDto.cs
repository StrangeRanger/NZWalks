namespace NZWalks.API.Dto.Domain;

/*
 * We implement the DTOs, despite it feeling redundant, because it allows us to control what data is sent to the
 * client. This will allow flexibility in the future if we decide to change the structure of our domain models, and
 * adds a layer of security by not exposing all of our domain model's properties, and separating the concerns of our
 * domain models and the data we send to the client.
 */
public class RegionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}