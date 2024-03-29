﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ColorHexadecimal { get; set; } = "#00c2a2";

        public DateTime DateCreated { get; set; }

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }

        public virtual ICollection<Note> Note { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}

