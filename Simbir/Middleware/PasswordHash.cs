using Konscious.Security.Cryptography;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace Simbir.middleware
{
    public class PasswordHash
    {
        // Метод для создания случайной соли
        public string CreateSalt()
        {
            var buffer = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
            }
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }

        // Метод для хеширования пароля с использованием Argon2id
        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromHexString(salt);
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = saltBytes;
                argon2.DegreeOfParallelism = 2; // количество потоков
                argon2.Iterations = 2; // количество итераций
                argon2.MemorySize = 216 * 216; // объем памяти в килобайтах
                byte[] hash = argon2.GetBytes(32); // длина хеша в байтах
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // Метод для сравнения хеша и пароля
        public  bool VerifyPassword(string password, string salt, string hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash); // возвращает true, если хеши равны
        }
    }
}
