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

        [Required]
        public string ColorHexadecimal = string.Empty;

        public DateTime DateCreated { get; set; }

        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
    }
}
