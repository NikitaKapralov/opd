using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EkbCulture.AppHost.Models
{
    public class User
    {
        public User()
        {
            // Инициализация по умолчанию
            Level = 1;
            VisitedLocations = Array.Empty<int>();
            IconUrl = "";
        }
        public User(string username,string mail, string hash)
        {
            Username = username;
            Email = mail;
            PasswordHash = hash;
            Level = 1;
            VisitedLocations = Array.Empty<int>();
            IconUrl = null;
        }
        public int Id { get; set; } //ID юзера в базе данных

        public string Username { get; set; } //никнейм

        public string Email { get; set; } //email (для авторизации)

        public string PasswordHash { get; set; } // Храним только хеш

        public int Level { get; set; } = 1;

        public int[] VisitedLocations { get; set; } = Array.Empty<int>();

        public string? IconUrl { get; set; } = "";

        public void UpdateLevel() =>
        Level = (VisitedLocations?.Length ?? 0) / 5 + 1;

    }
}