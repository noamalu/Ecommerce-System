using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;
using Moq;
using MarketBackend.Domain.Shipping;
using MarketBackend.Domain.Payment;

namespace UnitTests
{
    [TestClass]
    public class StoreTest
    {
        private Client _owner;
        private Store _Store;
        private Product _p1;

        [TestInitialize]
        public void Initialize()
        {
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            MarketService s = MarketService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            ClientService c = ClientService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            ClientManager CM = ClientManager.GetInstance();
            MarketManagerFacade MMF = MarketManagerFacade.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            c.Register(2, "nofar", "12345", "nofar@gmail.com", 19);
            c.LoginClient(2, "nofar", "12345");
            int storeId= MMF.CreateStore(2, "shop1", "shop@gmail.com", "0502552798");
            _owner = CM.GetClientById(2);
            _Store = MMF.GetStore(storeId);
            _Store.AddProduct(_owner.Id, "Brush", "" , "Brush", 4784, "hair", 21, false);
            _p1 = _Store.Products.ToList().Find((p) => p.ProductId == 11);
            c.Register(3, "Noa", "54321", "nofar@gmail.com", 18);
            c.LoginClient(3, "Noa", "54321");
        }

         [TestCleanup]
        public void Cleanup()
        {
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            MarketService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object).Dispose();
            ClientService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object).Dispose();
        
        }

        [TestMethod()]
        public void AddProductSuccess()
        {
            _Store.AddProduct(_owner.Id, "Shampo", "" , "Shampo", 4784, "hair", 21, false);
            Assert.IsTrue(_Store.Products.ToList().Find((p) => p.Name == "Shampo") != null);
        }

        [TestMethod()]
        public void AddProductFailHasNoPermissions()
        {
            Assert.ThrowsException<Exception>(() => _Store.AddProduct(3,"Shampo", "" , "Shampo", 4784, "hair", 21, false));
        }

        [TestMethod()]
        public void AddProductFailUserNotExist()
        {
            Assert.ThrowsException<Exception>(() => _Store.AddProduct(17,"Shampo", "" , "Shampo", 4784, "hair", 21, false));
        }

        [TestMethod()]
        public void RemoveProductSuccess()
        {
            _Store.AddProduct(_owner.Id, "Shampo", "" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            _Store.RemoveProduct(_owner.Id, p1.ProductId);
            Assert.IsTrue(!_Store.Products.Contains(p1));
        }

        [TestMethod()]
        public void RemoveProductFailNOPrermissions()
        {
           _Store.AddProduct(_owner.Id, "Shampo", "" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            Assert.ThrowsException<Exception>(() => _Store.RemoveProduct(3, p1.ProductId));
        }

        [TestMethod()]
        public void RemoveProductFailUserNotExist()
        {
           _Store.AddProduct(_owner.Id, "Shampo", "" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            Assert.ThrowsException<Exception>(() => _Store.RemoveProduct(17, p1.ProductId));
        }

        [TestMethod()]
        public void OpenStoreSuccess()
        {
            _Store.Active = false;
            _Store.OpenStore(_owner.Id);
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public void OpenStoreFailUserNotExist()
        {
            _Store.Active = false;
            Assert.ThrowsException<Exception>(() => _Store.OpenStore(17));
            Assert.IsFalse(_Store.Active);
        }

        [TestMethod()]
        public void OpenStoreFailUserHasNoPermissions()
        {
            _Store.Active = false;
            Assert.ThrowsException<Exception>(() => _Store.OpenStore(3));
            Assert.IsFalse(_Store.Active);
        }
        [TestMethod()]
        public void OpenStoreFailStoreIsOpen()
        {
            _Store.Active = true;
            Assert.ThrowsException<Exception>(() => _Store.OpenStore(_owner.Id));
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public void closeStoreSuccess()
        {
            _Store.Active = true;
            _Store.CloseStore(_owner.Id);
            Assert.IsFalse(_Store.Active);
        }

        [TestMethod()]
        public void CloseStoreFailUserNotExist()
        {
            _Store.Active = true;
            Assert.ThrowsException<Exception>(() => _Store.CloseStore(17));
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public void CloseStoreFailUserHasNoPermissions()
        {
            _Store.Active = true;
            Assert.ThrowsException<Exception>(() => _Store.CloseStore(3));
            Assert.IsTrue(_Store.Active);
        }
        [TestMethod()]
        public void CloseStoreFailStoreIsClose()
        {
            _Store.Active = false;
            Assert.ThrowsException<Exception>(() => _Store.CloseStore(_owner.Id));
            Assert.IsFalse(_Store.Active);
        }

         public void UpdateProductPriceSuccess()
        {
            _Store.UpdateProductPrice(_owner.Id, _p1.ProductId, 45555);
            Assert.IsTrue(_p1.Price == 45555);
        }

        [TestMethod()]
        public void UpdateProductPriceFailUserNotExist()
        {
            Assert.ThrowsException<Exception>(() => _Store.UpdateProductPrice(14, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public void UpdateProductPriceFailUserHasNotPermissions()
        {
            Assert.ThrowsException<Exception>(() => _Store.UpdateProductPrice(3, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public void UpdateProductQuantitySuccess()
        {
            _Store.UpdateProductQuantity(_owner.Id, _p1.ProductId, 45555);
            Assert.IsTrue(_p1.Quantity == 45555);
        }

        [TestMethod()]
        public void UpdateProductQuantityFailUserNotExist()
        {
            Assert.ThrowsException<Exception>(() => _Store.UpdateProductQuantity(14, _p1.ProductId, 45555));
        }

         [TestMethod()]
        public void UpdateProductQuantityFailUserHasNotPermissions()
        {
            Assert.ThrowsException<Exception>(() => _Store.UpdateProductQuantity(3, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public void AddStaffMemberSuccess()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, 3);
            _Store.AddStaffMember(3, role, _owner.Id);
            Assert.IsTrue(_Store.roles.ContainsValue(role));

        }

        [TestMethod()]
        public void AddStaffMemberFailUserNotExist()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, 3);
            Assert.ThrowsException<Exception>(() => _Store.AddStaffMember(3, role, 17));
            Assert.IsFalse(_Store.roles.ContainsValue(role));

        }

        [TestMethod()]
        public void AddStaffMemberFailUserHasNoPermissions()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, 3);
            Assert.ThrowsException<Exception>(() => _Store.AddStaffMember(3, role, 3));
            Assert.IsFalse(_Store.roles.ContainsValue(role));
        }

        [TestMethod()]
        public void AddStaffMemberFailAnotherFounder()
        {
            Role role = new Role(new Founder(RoleName.Founder), (Member)_owner, _Store._storeId, 3);
            Assert.ThrowsException<Exception>(() => _Store.AddStaffMember(3, role, _owner.Id));
            Assert.IsFalse(_Store.roles.ContainsValue(role));

        }

        [TestMethod()]
        public void PurchaseBasketSuccess()
        {
            Basket basket = new Basket(13, _Store._storeId);
            basket.addToBasket(11, 10);
            Purchase purchase = _Store.PurchaseBasket(3,basket);
            Assert.IsTrue(_Store.Purchases.Contains(purchase));
            Product product = _Store.GetProduct(11);
            Assert.IsTrue(product._quantity == 11);
        }

        [TestMethod()]
        public void PurchaseBasketFailStoreIsClose()
        {
            Basket basket = new Basket(13, _Store._storeId);
            basket.addToBasket(11, 10);
            _Store._active=false;
            Assert.ThrowsException<Exception>(() => _Store.PurchaseBasket(3,basket));
            Product product = _Store.GetProduct(11);
            Assert.IsTrue(product._quantity == 21);
            
        }

         [TestMethod()]
        public void PurchaseBasketFailnotEnoughQuantity()
        {
            Basket basket = new Basket(13, _Store._storeId);
            basket.addToBasket(11, 40);
            Assert.ThrowsException<Exception>(() => _Store.PurchaseBasket(3,basket));
            Product product = _Store.GetProduct(11);
             Assert.IsTrue(product._quantity == 21);
            
        }



 




    }
}