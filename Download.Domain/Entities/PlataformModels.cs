
using System.Collections.ObjectModel;

namespace Download.Domain.Entities
{
    public sealed class PlataformModels  : Entity
    {
        public string? PlatformName { get; private set; } = string.Empty;

        public string? Slug { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public string? PlatformDescription { get; private set; } = string.Empty;

        public string? PlataformDeveloper { get; private set; } = string.Empty;



        public string? PlataformGamesBoxArt { get; private set; } = string.Empty;

        public string? PlataformGamesSnipet { get; private set; } = string.Empty;

        public string? PlataformGamesNamedTitle { get; private set; } = string.Empty;


        public string? PlataformGameImage { get; private set; } = string.Empty;

        public string? PlatafomGameLogo { get; private set; } = string.Empty;

        public string? PlataformGameImageInternalUrl { get; private set; } = string.Empty;

        public string? PlatafomGameLogoInternalUrl { get; private set; } = string.Empty;


        public int TotalGames { get; private set; } = 0;

        public int Count { get; private set; } = 0;



        public string? PlataformGamesFolder { get; private set; } = string.Empty;


        public IList<GameModel>? GameModels { get; set; }


        public ICollection<EmulatorsModel>? EmulatorsModels { get; set; }

        public ICollection<GameCategoryModel>? GameCategoryModels { get; set; }
       
        

    }
}
