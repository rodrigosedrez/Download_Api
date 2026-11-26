
namespace Download.Domain.Entities
{
    public sealed class MagazineModel : Entity
    {
        public string? MagazineName { get; private set; } = string.Empty;

        public string? MagazineDescription { get; private set; } = string.Empty;

        public string? MagazineUrl { get; private set; } = string.Empty;



    }
}
