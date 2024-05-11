using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Security
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