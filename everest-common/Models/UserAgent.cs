using System;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgent
    {
        public Guid Id = Guid.NewGuid();
        public string Key = string.Empty;
        public DateTime LastPing = DateTime.UtcNow;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}

