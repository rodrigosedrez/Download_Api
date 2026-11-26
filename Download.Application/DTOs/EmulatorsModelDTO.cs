
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
    public class EmulatorsModelDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Coloque uma Emulator Name")]
        public string? EmulatorName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? EmulatorVersion { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Slug { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        [MaxLength(1000)]
        public string? EmulatorDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? EmulatorOS { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? EmulatorInternalUrl { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? EmulatorUrlOficial { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? EmulatorFolder { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? EmulatorLogo { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? EmulatorSnipet { get; set; } = string.Empty;
          [MaxLength(300)]
        public string? EmulatorLogoInternalUrl { get;  set; } = string.Empty;
          [MaxLength(300)]
        public string? EmulatorSnipetInternalUrl { get;  set; } = string.Empty;


        public int PlataformModelsId { get; set; }


        [JsonIgnore]
        [ForeignKey("PlataformModelsId")]
        public PlataformModelsDTO? PlataformModels { get; set; }




    }
}
