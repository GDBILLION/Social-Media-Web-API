﻿using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApi.DTOs.Account
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
