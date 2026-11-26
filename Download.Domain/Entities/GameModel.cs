
namespace Download.Domain.Entities
{
    public sealed class GameModel  : Entity
    {
        public string GameName { get; private set; } = string.Empty;

        public string? Slug { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public string GameDescription { get; private set; } = string.Empty;

        public string GameLenguage { get; private set; } = string.Empty;

        public string GameOriginalOrMode { get; private set; } = string.Empty;

        public string ImageBoxArt { get; private set; } = string.Empty;

        public string ImageSnaps { get; private set; } = string.Empty;

        public string NamedTitle { get; private set; } = string.Empty;


        public string ImageBoxArtUrlInternal { get; private set; } = string.Empty;

        public string ImageSnapsUrlInternal { get; private set; } = string.Empty;

        public string NamedTitleUrlInternal { get; private set; } = string.Empty;

        public string Video1 { get; private set; } = string.Empty;

        public string Video2 { get; private set; } = string.Empty;

        public string Video3 { get; private set; } = string.Empty;
        public int Count { get; private set; } = 0;

        public int PlataformId { get; private set; } // FK to PlataformModels

        public string PlataformSlug { get; private set; } = string.Empty;


        public PlataformModels? PlataformModels { get; set; }

         public List<int>? GameCategoryModelIds { get; set; } // FK to GameCategoryModels

       
        public ICollection<GameCategoryModel>? GameCategoryModels { get; set; } 


 



    }
}
