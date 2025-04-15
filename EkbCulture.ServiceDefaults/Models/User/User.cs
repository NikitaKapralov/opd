using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.ServiceDefaults.Models.User
{
    public class User
    {
        private int Id { get; set; } //ID юзера в базе данных (скрыто от посторонних глаз)

        public int Username { get; set; } //никнейм

        public int Level { get; set; } // "уровень" по посещенным местам

        public List<int> VisitedPlacesId { get; set; } //список посещенных мест 

        private string email { get; set; } //email (для авторизации)

        private string phoneNumber { get; set; } //номер телефона (для авторизации)

        private HashCode passwordHash { get; } //пароли будут храниться в виде хеша (для безопасности)

        private UserType userType = UserType.standardUser; //вид аккаунта ( подробнее в UserType.cs)
    }
}
