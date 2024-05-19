using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Security
{
    public interface ITokenManager
    {
        public string GenerateToken(int userId);
        public bool ValidateToken(string token);
    }
}
