using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services;
using MarketBackend.DAL;

namespace UnitTests
{
    [TestClass]
    public class StoreTest
    {
        private Client _owner;
        private Store _Store;
        private Product _p1;
        private Product _p2;
        private Product _p3;
        private Product _p4;

        [TestInitialize]
        public void Initialize()
        {
            MarketService s = MarketService.GetInstance();
            ClientService c = ClientService.GetInstance();
            MarketContext.GetInstance().Dispose();
            MarketContext context = MarketContext.GetInstance();
            ClientManager CM = ClientManager.GetInstance();
            MarketManagerFacade MMF = MarketManagerFacade.GetInstance();
            MarketManager MM = MarketManager.GetInstance();
            var mockDeliverySystem = new Mock<IDeliverySystem>();
            var mockPaymentSystem = new Mock<IPaymentSystem>();
            mockDeliverySystem.Setup(d => d.Connect())
             .Returns(true);
            mockPaymentSystem.Setup(d => d.Connect())
             .Returns(true);
            c.Register(2, "nofar", "12345", "nofar@gmail.com", 19);
            c.LoginClient(2, "nofar", "12345");
            int storeId= MMF.CreateStore(2, "shop1", "shop@gmail.com", "0502552798");
            _owner = CM.GetClientById(2);
            _Store = MMF.GetStore(storeId);
            s.AddProduct("2", _Store.StoreId, "Ball",0, "this is a ball", 52.6, 80, Category.None.ToString(), new List<string> { "soccer", "basketball", "round" });
            s.AddProduct("2",  _Store.StoreId, "Ball1",0, "this is a ball1", 52.6, 80, Category.Pockemon.ToString(), new List<string> { "basketball", "round", "Pockemon" });
            s.AddProduct("2",  _Store.StoreId, "Ball2",0, "this is a ball2", 52.6, 80, Category.None.ToString(), new List<string>());
            s.AddProduct("2",  _Store.StoreId, "Ball3",0, "this is a ball3", 52.6, 80, Category.Furnitures.ToString(), new List<string> { "table" });
            _p1 = _shop.Products.ToList().Find((p) => p.Id == 11);
            _p2 = _shop.Products.ToList().Find((p) => p.Id == 12);
            _p3 = _shop.Products.ToList().Find((p) => p.Id == 13);
            _p4 = _shop.Products.ToList().Find((p) => p.Id == 14);
            s.Register("3", "nofar", "54321");
            s.Register("4", "noa", "111111");
            s.Register("5", "hadas", "22222");
            s.Register("6", "joni", "111111");
            s.Login("3", "nofar", "54321");
            s.Login("4", "noa", "111111");
            s.Login("5", "hadas", "22222");
            s.Login("6", "joni", "111111");
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



    }
}