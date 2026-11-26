
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
    public class PlataformModelsDTO
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Coloque uma Platform Name")]
        public string? PlatformName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Slug { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        [MaxLength(900)]
        [Required(ErrorMessage = "Coloque uma descrição para plataforma")]
        public string? PlatformDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? PlataformDeveloper { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? PlataformGamesBoxArt { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? PlataformGamesSnipet { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? PlataformGamesNamedTitle { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? PlataformGamesFolder { get;  set; } = string.Empty;


        [MaxLength(200)]
        public string? PlataformGameImage { get;  set; } = string.Empty;
        [MaxLength(200)]
       
        public string? PlatafomGameLogo { get;  set; } = string.Empty;
        
        [MaxLength(200)]
       
        public string? PlataformGameImageInternalUrl { get;  set; } = string.Empty;
         
        [MaxLength(200)]
        public string? PlatafomGameLogoInternalUrl { get;  set; } = string.Empty;



        public int? TotalGames { get; set; } = 0;

        public int Count { get;  set; } = 0;


        [JsonIgnore]
        public IList<GameModelDTO>? GameModels { get; set; } //= new List<GameModel>();

        [JsonIgnore]
        public ICollection<EmulatorsModelDTO>? EmulatorsModels { get; set; }
        [JsonIgnore]
        public ICollection<GameCategoryModelDTO>? GameCategoryModels { get; set; } 

    }
}
