
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
    public class MagazineModelDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "E preciso fornescer um nome para a revista")]
        public string? MagazineName { get; set; } = string.Empty;
        [MaxLength(300)]
        [Required(ErrorMessage = "E preciso fornescer uma breve descrição para a revista")]
        public string? MagazineDescription { get; set; } = string.Empty;

        [MaxLength(300)]
        [Required(ErrorMessage = "E preciso fornescer um URL para a revista")]
        public string? MagazineUrl { get; set; } = string.Empty;


    }
}
