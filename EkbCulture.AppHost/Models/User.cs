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
        public int Id { get; set; } //ID юзера в базе данных

        public string Username { get; set; } //никнейм

        public string Email { get; set; } //email (для авторизации)

        public string PasswordHash { get; set; } // Храним только хеш

        public int Level { get; set; }

        public int[] VisitedLocations { get; set; } = Array.Empty<int>();

        public string Icon { get; set; }

        public void UpdateLevel() =>
        Level = (VisitedLocations?.Length ?? 0) / 5 + 1;

    }
}