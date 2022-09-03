using System;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public DateTime LastPing { get; set; } = DateTime.UtcNow;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}

