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
    [Table("Roles")]
    public class RoleDTO
    {
        [Key, Column(Order = 0)]
        [ForeignKey("StoreDTO")]
        public int storeId { get; }
        
        [Key, Column(Order = 1)]
        [ForeignKey("MemberDTO")]
        public string userName { get; }

        public RoleTypeDTO role { get; }
        public MemberDTO appointer;
        public List<MemberDTO> appointees;

        public RoleDTO(Role role)
        {
            storeId = role.storeId;
            userName = role.userName;
            this.role = new RoleTypeDTO(role.role);
            appointer = new MemberDTO(role.appointer);
            appointees = new List<MemberDTO>();
            foreach (Member member in role.appointees)
                appointees.Add(new MemberDTO(member));
        }

        public RoleDTO(){}

        public static Role ConvertToRole(RoleDTO roleDto)
        {
            // Assuming you have a method to convert `RoleTypeDTO` to `RoleType`
            RoleType roleType = RoleTypeDTO.ConvertToRoleType(roleDto.role);
            
            // Assuming `MemberDTO` has a conversion constructor or method that doesn't require a full `Member` object
            Member appointer = new Member(roleDto.appointer);

            Role role = new Role(roleType, appointer, roleDto.storeId, roleDto.userName);

            // Handling list of appointees
            foreach (MemberDTO appDto in roleDto.appointees)
            {
                Member app = new Member(appDto);
                role.addAppointee(app);
            }

            return role;
        }

    }
}
