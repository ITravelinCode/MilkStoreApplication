using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Util
    {
        private static Util? util;
        public Util()
        {
        }

        public static Util GetInstance()
        {
            if (util == null)
            {
                util = new Util();
            }
            return util;
        }

        public int? ValidateJwtToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var accountId = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            return accountId != null ? int.Parse(accountId) : (int?)null;
        }
        public static String HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}
