
namespace Download.Domain.Entities
{
    public sealed class ExternalLinksModel : Entity
    {

        public string? ExternalLinkName { get; private set; } = string.Empty;

        public string? ExernalLinkDescription { get; private set; } = string.Empty;

        public string? ExternalLinkUrl { get; private set; } = string.Empty;

    }
}
