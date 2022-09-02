using System;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; } = string.Empty;
        public DateTime LastPing { get; set; } = DateTime.UtcNow;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}

