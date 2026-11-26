


namespace Download.Domain.Entities
{
    public sealed class EmulatorsModel : Entity
    {

        public string? EmulatorName { get; private set; } = string.Empty;

        public string? EmulatorVersion { get; private set; } = string.Empty;

        public string? Slug { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        public string? EmulatorDescription { get; private set; } = string.Empty;

        public string? EmulatorOS { get; private set; } = string.Empty;

        public string? EmulatorInternalUrl { get; private set; } = string.Empty;

        public string? EmulatorUrlOficial { get; private set; } = string.Empty;

        public string? EmulatorFolder { get; private set; } = string.Empty;

        public string? EmulatorLogo { get; private set; } = string.Empty;

        public string? EmulatorSnipet { get; private set; } = string.Empty;

        public string? EmulatorLogoInternalUrl { get; private set; } = string.Empty;

        public string? EmulatorSnipetInternalUrl { get; private set; } = string.Empty;






        public int PlataformModelsId { get; private set; }

        public PlataformModels? PlataformModels { get; set; }



    }
}
