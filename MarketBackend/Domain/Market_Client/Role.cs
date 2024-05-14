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
        private RoleType role;
        public Member appointer;
        public List<Member> appointees;

        public Role(RoleType role, Member appointer) { 
            this.role = role;
            this.appointer = appointer;
            appointees = new List<Member>();
        }

        public RoleType getRole() { return role; }
        public void setRole(RoleType newRole) { role = newRole; }

        public Member getAppointer() { return appointer; }

        public IReadOnlyList<Member> getAppointees() {  return appointees; }

        public void addAppointee(Member appToAdd) {  appointees.Add(appToAdd);}
        public void removeAppointee(Member appToRem) { appointees.Add(appToRem); }

        public virtual bool canAddProduct() { return role.canAddProduct(); }
        public virtual bool canRemoveProduct() { return role.canRemoveProduct(); }
        public virtual bool canOpenStore() { return role.canOpenStore(); }
        public virtual bool canCloseStore() { return role.canCloseStore(); }
        public virtual bool canUpdateProductPrice() { return role.canUpdateProductPrice(); }
        public virtual bool canUpdateProductDiscount() { return role.canUpdateProductDiscount(); }
        public virtual bool canUpdateProductQuantity() { return role.canUpdateProductQuantity(); }
        public virtual bool canAddStaffMember() { return role.canAddStaffMember(); }
        public virtual bool canRemoveStaffMember() { return role.canRemoveStaffMember(); }

        public virtual bool hasPermission(Permission permission)
        {
            return role.hasPermission(permission);
        }

        public virtual void addPermission(Permission permission)
        {
            role.addPermission(permission);
        }
        public virtual void removePermission(Permission permission)
        {
            role.removePermission(permission);
        }

        public virtual IReadOnlyCollection<Permission> getPermissions()
        {
            return role.getPermissions();
        }

    }
}
