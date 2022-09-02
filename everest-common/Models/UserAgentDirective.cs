using everest_common.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class UserAgentDirective
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public UInt64? SecondsToRun { get; set; }
        public UserAgentDirectiveStatus Status { get; set; } = UserAgentDirectiveStatus.NotRunning;

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }

        public Guid UserAgentId { get; set; }
        public UserAgent UserAgent { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

