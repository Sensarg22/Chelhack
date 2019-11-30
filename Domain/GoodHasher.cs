using System;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;

namespace Domain
{
    public static class GoodHasher
    {
        public static string Calculate(Good good)
        {
            var value = GetNormalizedValue(good);
            return CalculateInternal(value);
        }

        private static string CalculateInternal(string value)
        {
            var hashProvider = MD5.Create();
            var bytes = hashProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
            var hash = new Guid(bytes);
            return hash.ToString("N");
        }

        private static string GetNormalizedValue(Good good)
        {
            return good.ToString().ToLower();
        }
    }
}