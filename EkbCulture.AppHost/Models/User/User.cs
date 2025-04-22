using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using EkbCulture.AppHost.Models.User;

namespace EkbCulture.AppHost.Models.User
{
    public class User
    {
        public int Id { get; set; } //ID юзера в базе данных

        public string Username { get; set; } //никнейм

        public string email { get; set; } //email (для авторизации)

        
    }
}