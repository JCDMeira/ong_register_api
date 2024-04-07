using System.ComponentModel.DataAnnotations;

namespace OngResgisterApi.Models;

    public class Ong : MongoBaseEntity
    {
    public Ong(string name,string? description, string? imageUrl, string purpose, string[] howToAssist)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Purpose = purpose;
        HowToAssist = howToAssist;
    }

    public string? Description { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    [MaxLength(10)]
    public string[] HowToAssist { get;  set; } = null!;
}

