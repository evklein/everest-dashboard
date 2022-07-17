using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public string UpdatedName { get; set; } = string.Empty;

        public bool Complete { get; set; } = false;

        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

