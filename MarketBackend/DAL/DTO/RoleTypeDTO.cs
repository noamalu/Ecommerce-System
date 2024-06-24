using MarketBackend.Domain.Market_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MarketBackend.DAL.DTO
{
    [Table("ToleTypes")]
    public class RoleTypeDTO
    {
        [Key]
        [ForeignKey("MemberDTO")]
        public string userName { get; set; }

        [Key]
        [ForeignKey("StoreDTO")]
        public int storeId { get; set; }

        public string roleName { get; set; }
        public List<string> permissions {get; set; }

        public RoleTypeDTO(string username, int storeid ,RoleType roleType)
        {
            storeId = storeid;
            userName = username;
            roleName = roleType.roleName.ToString();
            permissions = new List<string>();
            foreach (Permission permission in roleType.getPermissions())
                permissions.Add(permission.ToString());
        }
    }
}
