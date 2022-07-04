﻿using System.ComponentModel.DataAnnotations;

namespace BillingAPI.API.Account.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
