using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public class Member : Client
    {
        public string UserName {get; set;}
        public string Password {get; set;}
        public MailAddress Email {get; set;}
        public ConcurrentDictionary<int, Role> Roles {get; set;}
        public bool IsSystemAdmin {get; set;}
        public bool IsLoggedIn {get; set;}
        public Member(int id, string userName, MailAddress mailAddress, string password) : base(id)
        {
            UserName = userName;
            Password = password;
            Email = mailAddress;
            Roles = new();  
            IsSystemAdmin = false;
            IsLoggedIn = false;
        }

        

    }
    
}