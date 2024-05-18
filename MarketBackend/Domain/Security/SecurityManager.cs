using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace MarketBackend.Domain.Security
{
    public class SecurityManager : ISecurityManager
    {
        PasswordHasher<object> passwordHasher;
        public SecurityManager() { 
            passwordHasher = new PasswordHasher<object>();
        }
        public string EncryptPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            var result = passwordHasher.VerifyHashedPassword(null, rawPassword, hashedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}