using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public class StoreManager : RoleType
    {
        public StoreManager(string roleName) : base(roleName) { }

        public override bool canOpenStore() { return false; }

        public override bool canCloseStore() { return false; }

        public override bool canAddProduct()
        {
            return hasPermission(Permission.addProduct);
        }

        public override bool canRemoveProduct()
        {
            return hasPermission(Permission.removeProduct);
        }

        public override bool canUpdateProductPrice()
        {
            return hasPermission(Permission.updateProductPrice);
        }

        public override bool canUpdateProductQuantity()
        {
            return hasPermission(Permission.updateProductQuantity);
        }

        public override bool canUpdateProductDiscount()
        {
            return hasPermission(Permission.updateProductDiscount);
        }

        public override bool canAddStaffMember() { return false; }

        public override bool canRemoveStaffMember() { return false; }

    }
}
