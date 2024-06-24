using MarketBackend.Domain.Market_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.DAL.DTO
{
    [Table("Baskets")]
    public class RoleDTO
    {
        [Key]
        [ForeignKey("StoreDTO")]
        public int storeId { get; }
        
        [Key]
        [ForeignKey("MemberDTO")]
        public string userName { get; }

        public RoleTypeDTO role { get; }
        public MemberDTO appointer;
        public List<MemberDTO> appointees;

        public RoleDTO(Role role)
        {
            storeId = role.storeId;
            userName = role.userName;
            role = new RoleTypeDTO(role.role);
            appointer = new MemberDTO(role.appointer);
            appointees = new List<MemberDTO>();
            foreach (Member member in role.appointees)
                appointees.Add(new MemberDTO(member));
        }

    }
}
