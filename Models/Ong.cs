namespace OngResgisterApi.Models;

    public class Ong : MongoBaseEntity
    {
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public string HowToAssist { get;  set; } = null!;
}

