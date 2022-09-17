using System;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ReadableName { get; set; } = string.Empty;
        public DateTime LastConnectionActivity { get; set; } = DateTime.MinValue;

        public string EverestPrivateKey { get; set; } = string.Empty;
        public string EverestPublicKey { get; set; } = string.Empty;

        public string UserAgentPublicKey { get; set; } = string.Empty;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}

