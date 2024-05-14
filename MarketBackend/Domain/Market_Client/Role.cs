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
        public Member appointer; //todo: maybe change to ids rather than member?
        public List<Member> appointees;

        public Role(RoleType role, Member appointer) { 
            this.role = role;
            this.appointer = appointer;
            appointees = new List<Member> { appointer };
        }

        public RoleType getRole() { return role; }
        public void setRole(RoleType newRole) { role = newRole; }

        public Member getAppointer() { return appointer; }

        public IReadOnlyList<Member> getAppointees() {  return appointees; }

        public void addAppointee(Member appToAdd) {  appointees.Add(appToAdd);}
        public void removeAppointee(Member appToRem) { appointees.Add(appToRem); }



    }
}
