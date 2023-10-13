using System.Buffers.Text;
using System.Security.Cryptography;

namespace Simbir.middleware
{
    public class PasswordHash
    {
            private const int SaltSize = 16;
            private const int Iterations = 10000;
            private const int HashSize = 20;

            //Принимает пароль и возвращает его хеш в виде строки шестнадцатеричного формата
            public string PasswordHasher(string password)
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    var salt = new byte[SaltSize];

                    rng.GetBytes(salt);

                    using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                    {
                        var hash = pbkdf2.GetBytes(HashSize);

                        var result = new byte[SaltSize + HashSize];

                        Array.Copy(salt, 0, result, 0, SaltSize);
                        Array.Copy(hash, 0, result, SaltSize, HashSize);

                        return BitConverter.ToString(result).Replace("-", "");
                    }
                }
            }

            // принимает пароль и сохраненный хеш и проверяет их соответствие
            public bool VerifyPassword(string password, string hashedPassword)
            {
                var bytes = new byte[hashedPassword.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hashedPassword.Substring(i * 2, 2), 16);
                }

                var salt = new byte[SaltSize];
                Array.Copy(bytes, 0, salt, 0, SaltSize);

                var hash = new byte[HashSize];
                Array.Copy(bytes, SaltSize, hash, 0, HashSize);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    var computedHash = pbkdf2.GetBytes(HashSize);

                    for (int i = 0; i < HashSize; i++)
                    {
                        if (computedHash[i] != hash[i])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        
    }
}
