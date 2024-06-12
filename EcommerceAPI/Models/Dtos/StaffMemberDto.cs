using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Models.Dtos
{
    public class StaffMemberDto
    {
        public string? MemberUserName { get; set; }
        public string? Permission { get; set; }
        public string? RoleName { get; set; }
        public bool IsValid()
        {
            return MemberUserName is not null && (Permission is not null || RoleName is not null);
        }

    }
}