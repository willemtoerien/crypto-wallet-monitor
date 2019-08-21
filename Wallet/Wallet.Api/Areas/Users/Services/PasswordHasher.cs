﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;

namespace Wallet.Api.Areas.Userss.Services
{
    public class PasswordHasher
    {
        private const string Salt = "43ygtnTFS//6ak5sEE5Sbw==";

        public string Hash(string password)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password,
                Encoding.UTF8.GetBytes(Salt),
                KeyDerivationPrf.HMACSHA512,
                10000,
                256 / 8);
            return Convert.ToBase64String(valueBytes);
        }

        public bool Verify(string hashedPassword, string password)
        {
            return Hash(password) == hashedPassword;
        }
    }
}
