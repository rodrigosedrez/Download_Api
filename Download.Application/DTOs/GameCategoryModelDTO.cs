
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
    public class GameCategoryModelDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Coloque um nome para GameCategory ")]
        public string? GameCategoryModelName { get; set; } = string.Empty;
        [MaxLength(300)]
        [Required(ErrorMessage = "Coloque um descrição para GameCategory ")]
        public string? GameCategoryModelDescription { get; set; } = string.Empty;


        public int? PlataformId { get; set; } // FK to PlataformModels

        [JsonIgnore]
        [ForeignKey("PlataformId")]
        public PlataformModelsDTO? PlataformModelssDTO { get; set; }


        [JsonIgnore]
        public ICollection<GameModelDTO>? GameModelssDTO { get; set; }

    }
}
