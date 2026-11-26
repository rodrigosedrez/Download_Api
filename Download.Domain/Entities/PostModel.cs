
namespace Download.Domain.Entities
{
    public sealed class PostModel : Entity 
    {
       
          public string? Title { get; private set; } = string.Empty;


        public string? Slug { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public string? Paragraph1 { get; private set; } = string.Empty;
        public string? Paragraph2 { get; private set; } = string.Empty;
        public string? Paragraph3 { get; private set; } = string.Empty;
        public string? Paragraph4 { get; private set; } = string.Empty;
        public string? Paragraph5 { get; private set; } = string.Empty;

        public string? Strong1 { get; private set; } = string.Empty;
        public string? Strong2 { get; private set; } = string.Empty;
        public string? Strong3 { get; private set; } = string.Empty;
        public string? Strong4 { get; private set; } = string.Empty;
        public string? Strong5 { get; private set; } = string.Empty;


        public string? ParagrathPreview { get; private set; } = string.Empty;

          public string? ImageUrl1 { get; private set; } = string.Empty;

          public string? ImageUrl2 { get; private set; } = string.Empty;

          public string? ImageUrl3 { get; private set; } = string.Empty;
          public string? ImageUrl4 { get; private set; } = string.Empty;
          public string? ImageUrl5 { get; private set; } = string.Empty;


          public string? ImageUrl1InternalUrl { get; private set; } = string.Empty;

          public string? ImageUrl2InternalUrl { get; private set; } = string.Empty;

          public string? ImageUrl3InternalUrl { get; private set; } = string.Empty;
          public string? ImageUrl4InternalUrl { get; private set; } = string.Empty;
          public string? ImageUrl5InternalUrl { get; private set; } = string.Empty;

          public string? UrlVideo1 { get; private set; } = string.Empty;

          public string? UrlVideo2 { get; private set; } = string.Empty;



    }
}
