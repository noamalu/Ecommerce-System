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
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string? MemberUserName { get; set; }
        public string? Role { get; set; }
        public bool IsValid()
        {
            return Id != 0 && StoreId != 0 && MemberUserName is not null && Role is not null;
        }

    }
}