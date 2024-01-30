namespace OngResgisterApi.Models;

    public class Ong : MongoBaseEntity
    {
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
}

