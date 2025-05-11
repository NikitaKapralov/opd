using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.AppHost.Services
{
    public static class PasswordHasherService
    {
        //секретная циферка
        private const ulong INT_FOR_HASH = 1235921259127461922 ;
        // Хеширование пароля
        public static string HashPassword(string password)
        {
            ulong sum = 0;
            foreach (char c in password)
            {
                sum += (ulong)c.GetHashCode() * INT_FOR_HASH;
            }
            return sum.ToString();
        }

        // Проверка пароля
        public static bool VerifyPassword(string password, string hash)
        {
            var current = HashPassword(password);
            return current == hash;
        }
    }
}
