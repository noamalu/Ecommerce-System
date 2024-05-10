using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Facades.Interfaces
{
    public interface ISecurityManager
    {
        void EncryptPassword(string password);
        void VerifyPassword(string password);
    }
}