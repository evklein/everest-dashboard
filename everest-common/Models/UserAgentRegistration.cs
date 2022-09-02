using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgentRegistration
    {
        public Guid Id = Guid.NewGuid();
        public string Key = string.Empty;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}

