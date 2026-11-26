using System;
using System.Collections.Generic;
using System.Text;

namespace Download.Domain.Entities
{
    public class GameTagModel : Entity
    {
        public string TagName { get; private set; } = string.Empty;
        public string? Slug { get; private set; } = string.Empty;
        public string? TagPlatform { get; private set; } = string.Empty;
        public string? Description { get; private set; } = string.Empty;
        public int RequestCount { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
