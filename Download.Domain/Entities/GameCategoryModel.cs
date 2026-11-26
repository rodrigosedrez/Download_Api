

namespace Download.Domain.Entities
{
    public sealed class GameCategoryModel : Entity
    {
       
        public string? GameCategoryModelName { get; set; } = string.Empty;
        public string? GameCategoryModelDescription { get; set; } = string.Empty;


        public int? PlataformId { get; set; } // FK to PlataformModels

        public PlataformModels? PlataformModelss { get; set; }


      
        public ICollection<GameModel>? GameModelss { get; set; }

    }
}
