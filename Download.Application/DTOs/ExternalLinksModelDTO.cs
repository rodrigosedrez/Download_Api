

using System.ComponentModel.DataAnnotations;

namespace Download.Application.DTOs
{
    public class ExternalLinksModelDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "E preciso fornescer um nome para o link externo")]
        public string? ExternalLinkName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? ExernalLinkDescription { get; set; } = string.Empty;

        [MaxLength(200)]
        [Required(ErrorMessage = "E preciso fornescer um URL para o link externo")]
        public string? ExternalLinkUrl { get; set; } = string.Empty;

    }
}
