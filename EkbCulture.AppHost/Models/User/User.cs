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
        private int Id { get; set; } //ID юзера в базе данных (скрыто от посторонних глаз)

        public int Username { get; set; } //никнейм

        public int Level { get; set; } // "уровень" по посещенным местам

        public List<int> VisitedPlacesId { get; set; } //список посещенных мест 

        private string email { get; set; } //email (для авторизации)

        private string phoneNumber { get; set; } //номер телефона (для авторизации)

        private string passwordHash { get; } //пароли будут храниться в виде хеша (для безопасности)

        private readonly UserType userType = UserType.standardUser; //вид аккаунта ( подробнее в UserType.cs)
    }
}