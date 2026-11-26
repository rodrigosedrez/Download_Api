

using System.ComponentModel.DataAnnotations;

namespace Download.Application.DTOs
{
    public class PublicityDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string? PublicityName { get;  set; } = string.Empty;

        [MaxLength(100)]
        public string? Company { get;  set; } = string.Empty;

        [MaxLength(100)]
        public string? PublicityFolder { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required]
        public string? PublictyId { get; set; } = string.Empty;

        public DateTime? ReleaseDate { get; set; }
    }
}
