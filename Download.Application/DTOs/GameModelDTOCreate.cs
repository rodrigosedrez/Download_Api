
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
    public class GameModelDTOCreate
    {
        public int? Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Coloque um nome para o jogo")]
        public string GameName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Slug { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        [MaxLength(1000)]
        [Required(ErrorMessage = "Coloque uma descrição para o jogo")]
        public string GameDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required(ErrorMessage = "Coloque uma Lingua para o jogo")]
        public string GameLenguage { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? GameOriginalOrMode { get; set; } = string.Empty;

        [MaxLength(400)]
        public string ImageBoxArt { get; set; } = string.Empty;
        [MaxLength(400)]
        public string ImageSnaps { get; set; } = string.Empty;
        [MaxLength(400)]
        public string NamedTitle { get; set; } = string.Empty;

        [MaxLength(400)]
        public string ImageBoxArtUrlInternal { get; set; } = string.Empty;

        [MaxLength(400)]
        public string ImageSnapsUrlInternal { get; set; } = string.Empty;

        [MaxLength(400)]
        public string NamedTitleUrlInternal { get; set; } = string.Empty;

        [MaxLength(400)]
        public string Video1 { get; private set; } = string.Empty;
        [MaxLength(400)]
        public string Video2 { get; private set; } = string.Empty;
        [MaxLength(400)]
        public string Video3 { get; private set; } = string.Empty;

        public int? PlataformId { get; set; } // FK to PlataformModels

        public string PlataformSlug { get; set; } = string.Empty;
        public int Count { get;  set; }

        [JsonIgnore]
        [ForeignKey("PlataformId")]
        public PlataformModelsDTO? PlataformModels { get; set; }


        public List<int>? GameCategoryModelIds { get; set; } // FK to GameCategoryModels

        [JsonIgnore]
        [ForeignKey("GameCategoryModelIds")]
        public ICollection<GameCategoryModelDTO>? GameCategoryModels { get; set; }





    }
}
