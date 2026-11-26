
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Download.Application.DTOs
{
    public class GameTagModelDTO
    {
        public int Id { get; set; }
        

        [MaxLength(200)]
        public string TagName { get;  set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Slug { get;  set; } = string.Empty;


        [MaxLength(100)]
        public string? TagPlatform { get;  set; } = string.Empty;

        [MaxLength(900)]
        public string? Description { get;  set; } = string.Empty;

        public int RequestCount { get; set; } = 0;

        public DateTime CreatedAt { get;  set; } = DateTime.UtcNow;
    }
}
