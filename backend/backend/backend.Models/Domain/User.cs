﻿namespace backend.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string KeycloakId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
