using System.ComponentModel.DataAnnotations;

namespace OngResgisterApi.Entities.ViewModels
{
    public class OngViewlModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public string? ImageUrl { get; set; } = null!;

        public string Purpose { get; set; } = null!;

        [MaxLength(10)]
        public string[] HowToAssist { get; set; } = null!;

        public DateTime? CreatedTime { get; } = DateTime.Now!;

        public DateTime? UpdatedTime { get; set; } = DateTime.Now!;
    }
}
