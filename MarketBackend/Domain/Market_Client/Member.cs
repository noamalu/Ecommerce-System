using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public class Member : User
    {
        private string UserName {get; set;}
        private ConcurrentDictionary<int, Role> Roles {get; set;}
        private bool IsSystemAdmin {get; set;}
        private bool IsLoggedIn {get; set;}
        public Member(int id, string userName) : base(id)
        {
            UserName = userName;
            Roles = new();  
            IsSystemAdmin = false;
            IsLoggedIn = false;
        }

    }
}