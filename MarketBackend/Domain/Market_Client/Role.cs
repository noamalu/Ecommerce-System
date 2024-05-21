using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public class Role
    {
        public RoleType role { get; }
        public int storeId { get; }
        public int memberId { get; }
        public Member appointer;
        public List<Member> appointees;

        public Role(RoleType role, Member appointer, int storeId, int memberId) { 
            this.role = role;
            this.appointer = appointer;
            this.storeId = storeId;
            this.memberId = memberId;
            appointees = new List<Member>();
        }

        public Member getAppointer() { return appointer; }

        public IReadOnlyList<Member> getAppointees() {  return appointees; }

        public void addAppointee(Member appToAdd) {  appointees.Add(appToAdd);}
        public void removeAppointee(Member appToRem) { appointees.Add(appToRem); }

        public bool canAddProduct() { return role.canAddProduct(); }
        public bool canRemoveProduct() { return role.canRemoveProduct(); }
        public bool canOpenStore() { return role.canOpenStore(); }
        public bool canCloseStore() { return role.canCloseStore(); }
        public bool canUpdateProductPrice() { return role.canUpdateProductPrice(); }
        public bool canUpdateProductDiscount() { return role.canUpdateProductDiscount(); }
        public bool canUpdateProductQuantity() { return role.canUpdateProductQuantity(); }
        public bool canAddStaffMember(RoleName roleName) { return role.canAddStaffMember(roleName); }
        public bool canRemoveStaffMember(RoleName roleName) { return role.canRemoveStaffMember(roleName); }
        public bool canEditPermissions() { return role.canEditPermissions(); }

        public bool canGetHistory() { return role.canGetHistory(); }
        public bool hasPermission(Permission permission)
        {
            return role.hasPermission(permission);
        }

        public void addPermission(Permission permission)
        {
            role.addPermission(permission);
        }
        public void removePermission(Permission permission)
        {
            role.removePermission(permission);
        }

        public IReadOnlyCollection<Permission> getPermissions()
        {
            return role.getPermissions();
        }

        public RoleName getRoleName()
        {
            return role.roleName;
        }
    }
}
