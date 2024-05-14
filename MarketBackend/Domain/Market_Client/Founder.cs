using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    //founder has all permissions
    public class Founder : RoleType
    {
        public Founder(string roleName) : base(roleName) {
            permissions.Add(Permission.all);
        }

        public override void addPermission(Permission permission)
        {
            throw new Exception("can't change founder's permissions");
        }

        public override void removePermission(Permission permission)
        {
            throw new Exception("can't change founder's permissions");
        }
    }
}
