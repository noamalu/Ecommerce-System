using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Facades.Interfaces;

namespace MarketBackend.Domain.Facades
{
    public class SecurityManager : ISecurityManager
    {
        public void EncryptPassword(string password)
        {
            throw new NotImplementedException();
        }

        public void VerifyPassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}