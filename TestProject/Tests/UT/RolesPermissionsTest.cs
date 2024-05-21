using MarketBackend.Domain.Market_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class RolesPermissionsTest
    {
        Role founder;
        Role owner;
        Role manager;

        [TestInitialize]
        public void SetUp()
        {
            founder = new Role(new Founder(RoleName.Founder), null, 0, 0);
            owner = new Role(new Owner(RoleName.Owner), null, 0, 1);
            manager = new Role(new StoreManagerRole(RoleName.Manager), null, 0, 2);
        }

        [TestMethod]
        public void canAddProductSuccess()
        {
            manager.addPermission(Permission.addProduct);
            Assert.IsTrue(manager.canAddProduct());

            Assert.IsTrue(founder.canAddProduct());
            Assert.IsTrue(owner.canAddProduct());
        }

        [TestMethod]
        public void canAddProductFailure()
        {
            manager.addPermission(Permission.addProduct);
            manager.removePermission(Permission.addProduct);
            Assert.IsFalse(manager.canAddProduct());
        }

        [TestMethod]
        public void canRemoveProductSuccess()
        {
            manager.addPermission(Permission.removeProduct);
            Assert.IsTrue(manager.canRemoveProduct());

            Assert.IsTrue(owner.canRemoveProduct());
            Assert.IsTrue(founder.canRemoveProduct());
        }

        [TestMethod]
        public void canRemoveProductFailure()
        {
            manager.addPermission(Permission.removeProduct);
            manager.removePermission(Permission.removeProduct);
            Assert.IsFalse(manager.canRemoveProduct());
        }

        [TestMethod]
        public void canUpdateProductPriceSuccess()
        {
            manager.addPermission(Permission.updateProductPrice);
            Assert.IsTrue(manager.canUpdateProductPrice());

            Assert.IsTrue(founder.canUpdateProductPrice());
            Assert.IsTrue(owner.canUpdateProductPrice());
        }

        [TestMethod]
        public void canUpdateProductPriceFailure()
        {
            manager.addPermission(Permission.updateProductPrice);
            manager.removePermission(Permission.updateProductPrice);
            Assert.IsFalse(manager.canUpdateProductPrice());
        }

        [TestMethod]
        public void canUpdateProductDiscountSuccess()
        {
            manager.addPermission(Permission.updateProductDiscount);
            Assert.IsTrue(manager.canUpdateProductDiscount());

            Assert.IsTrue(founder.canUpdateProductDiscount());
            Assert.IsTrue(owner.canUpdateProductDiscount());
        }

        [TestMethod]
        public void canUpdateProductDiscountFailure()
        {
            manager.addPermission(Permission.updateProductDiscount);
            manager.removePermission(Permission.updateProductDiscount);
            Assert.IsFalse(manager.canUpdateProductDiscount());
        }

        [TestMethod]
        public void canUpdateProductQuantitySuccess()
        {
            manager.addPermission(Permission.updateProductQuantity);
            Assert.IsTrue(manager.canUpdateProductQuantity());

            Assert.IsTrue(founder.canUpdateProductQuantity());
            Assert.IsTrue(owner.canUpdateProductQuantity());
        }

        [TestMethod]
        public void canUpdateProductQuantityFailure()
        {
            manager.addPermission(Permission.updateProductQuantity);
            manager.removePermission(Permission.updateProductQuantity);
            Assert.IsFalse(manager.canUpdateProductQuantity());
        }

        [TestMethod]
        public void canCloseStoreTest()
        {
            Assert.IsFalse(manager.canCloseStore());
            Assert.IsFalse(owner.canCloseStore());
            Assert.IsTrue(founder.canCloseStore());
        }

        [TestMethod]
        public void canOpenStoreTest()
        {
            Assert.IsFalse(manager.canOpenStore());
            Assert.IsTrue(owner.canOpenStore());
            Assert.IsTrue(founder.canOpenStore());
        }

        [TestMethod]
        public void canAddStaffMemberManagerTest()
        {
            Assert.IsFalse(manager.canAddStaffMember(RoleName.Manager));
            Assert.IsFalse(manager.canAddStaffMember(RoleName.Owner));
            Assert.IsFalse(manager.canAddStaffMember(RoleName.Founder));
        }

        [TestMethod]
        public void canAddStaffMemberOwnerTest()
        {
            Assert.IsTrue(owner.canAddStaffMember(RoleName.Manager));
            Assert.IsTrue(owner.canAddStaffMember(RoleName.Owner));
            Assert.IsFalse(owner.canAddStaffMember(RoleName.Founder));
        }

        [TestMethod]
        public void canAddStaffMemberFounderTest()
        {
            Assert.IsTrue(founder.canAddStaffMember(RoleName.Manager));
            Assert.IsTrue(founder.canAddStaffMember(RoleName.Owner));
            Assert.IsFalse(founder.canAddStaffMember(RoleName.Founder));
        }

    }
}
