
namespace Download.Domain.Entities
{
    public class Publicity : Entity
    {
        public string? PublicityName { get; private set; } = string.Empty;
        public string? Company { get; private set; } = string.Empty;
        public string? PublicityFolder { get; private set; } = string.Empty;
        public string? PublictyId { get; private set; } = string.Empty;
        public DateTime? ReleaseDate { get; private set; }
    }
}
