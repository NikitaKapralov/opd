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

        /* другие параметры User`a (доработаю позже)
        public int Level { get; set; } // "уровень" по посещенным местам

        public List<int> VisitedPlacesId { get; set; } //список посещенных мест 

        private string phoneNumber { get; set; } //номер телефона (для авторизации)

        private string passwordHash { get; } //пароли будут храниться в виде хеша (для безопасности)

        private readonly UserType userType = UserType.standardUser; //вид аккаунта ( подробнее в UserType.cs)
        */
    }
}