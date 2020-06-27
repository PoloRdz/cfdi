using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class PasswordHasher
    {
        /// <summary>
        /// Tamaño del Salt.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Tamaño del Hash.
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Crea un has a partir de una contraseña.
        /// </summary>
        /// <param name="password">contraseña.</param>
        /// <param name="iterations">Número de iteraciones.</param>
        /// <returns>El hash de la contraseña.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt;
                rng.GetBytes(salt = new byte[SaltSize]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var hash = pbkdf2.GetBytes(HashSize);
                    // Combine salt and hash
                    var hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                    // Convert to base64
                    var base64Hash = Convert.ToBase64String(hashBytes);

                    // Format hash with extra information
                    return $"$HASH|V1${iterations}${base64Hash}";
                }
            }

        }

        /// <summary>
        /// Crea un Has de una contraseña con 10000 iteraciones.
        /// </summary>
        /// <param name="password">La contraseña.</param>
        /// <returns>El hash de la contraseña.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Verifica si el Hash es compatible.
        /// </summary>
        /// <param name="hashString">El Hash.</param>
        /// <returns>Compatibilidad</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("HASH|V1$");
        }

        /// <summary>
        /// Compara un Hash con una contraseña.
        /// </summary>
        /// <param name="password">La contraseña.</param>
        /// <param name="hashedPassword">El hash.</param>
        /// <returns>Si el Hash y contraseña coinciden</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("Este hash no es compatible");
            }

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$HASH|V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Get result
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
