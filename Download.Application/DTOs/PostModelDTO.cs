

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Download.Application.DTOs
{
     public class PostModelDTO
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Slug { get; set; } = string.Empty;


        [MaxLength(2000)]
        public string? Paragraph1 { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Paragraph2 { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Paragraph3 { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Paragraph4 { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Paragraph5 { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Strong1 { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? Strong2 { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? Strong3 { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? Strong4 { get;  set; } = string.Empty;

        [MaxLength(200)]
        public string? Strong5 { get;  set; } = string.Empty;

        [MaxLength(2000)]
        public string? ParagrathPreview { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        [MaxLength(300)]
        public string? ImageUrl1 { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? ImageUrl2 { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? ImageUrl3 { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? ImageUrl4 { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? ImageUrl5 { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? ImageUrl1InternalUrl { get;  set; } = string.Empty;

        [MaxLength(300)]
        public string? ImageUrl2InternalUrl { get;  set; } = string.Empty;

        [MaxLength(300)]
        public string? ImageUrl3InternalUrl { get;  set; } = string.Empty;
        
        [MaxLength(300)]
        public string? ImageUrl4InternalUrl { get;  set; } = string.Empty;

        [MaxLength(300)]
        public string? ImageUrl5InternalUrl { get;  set; } = string.Empty;

        [MaxLength(300)]
        public string? UrlVideo1 { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? UrlVideo2 { get; set; } = string.Empty;

    }
}
