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
        //секретные циферки, в релизе они будут совсем другими))
        private const ulong INT_FOR_HASH1 = 1222 ;
        private const ulong INT_FOR_HASH2 = 2642;
        private const ulong INT_FOR_HASH3 = 3592;
        // Хеширование пароля
        public static string HashPassword(string password)
        {
            ulong sum = 0;
            for (int i = 0; i < password.Length; i++)
            {
                ulong c = (ulong)password[i];
                sum += (((c * INT_FOR_HASH1) + INT_FOR_HASH2) * INT_FOR_HASH3);
            }
            return sum.ToString();
        }

        // Проверка пароля
        public static bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;
        
    }
}
