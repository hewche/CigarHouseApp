using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CigarHouseApp.Helpers
{
    public class PasswordHasher
    {
        public static string GetSHA512Hash(string input)
        {

            byte[] data;
            using (SHA512 sha512 = SHA512.Create())
            {
                data = sha512.ComputeHash(Encoding.Default.GetBytes(input));
            }
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        public static bool VerifySHA512Hash(string input, string hash)
        {
            string hashOfInput = GetSHA512Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
