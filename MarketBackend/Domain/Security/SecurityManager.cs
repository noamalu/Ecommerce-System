using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace MarketBackend.Domain.Security
{
    //change both managers to singletons
    public class SecurityManager : ISecurityManager, ITokenManager
    {
        PasswordHasher<object> passwordHasher;
        private ITokenManager tokenManager;
        public SecurityManager() { 
            passwordHasher = new PasswordHasher<object>();
            tokenManager = new TokenManager();
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

        public string GenerateToken(int userId)
        {
            return tokenManager.GenerateToken(userId);
        }
        public bool ValidateToken(string token)
        {
            return tokenManager.ValidateToken(token);
        }
    }
}