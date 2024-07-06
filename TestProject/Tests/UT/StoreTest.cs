using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;
using Moq;
using MarketBackend.Domain.Shipping;
using MarketBackend.Domain.Payment;
using MarketBackend.DAL.DTO;

namespace UnitTests
{
    [TestClass]
    public class StoreTest
    {
        private Client _owner;
        private Store _Store;
        private Product _p1;
        private string token1;
        private string token2;
        string username1 = "nofar";
        string username2 = "noa";
        string username3 = "yonatan";
        DBcontext context;

        private MarketManagerFacade marketManagerFacade;
        string userName = "user1";
        string session1 = "1";
        string userName2 = "user2";
        string session2 = "2";

        string userName3 = "user3";
        string session3 = "3";
        string token3;

        string userPassword = "pass1";
        string pass2 = "pass2";
        string email1 = "printz@post.bgu.ac.il";
        string email2 = "hadaspr100@gmail.com";
        string wrongEmail = "@gmail.com";
        int userId;
        int productID1 = 11;
        string productName1 = "Banana";
        string category1 = "Fruit";
        string storeName = "Remi levi";
        string phoneNum = "0522768972";
        double price1 = 5.0;
        int quantity1 = 10;
        double discount1 = 0.5; 
        int userAge = 20;
        int userAge2 = 16;
        int basketId = 1;
        PaymentDetails paymentDetails = new PaymentDetails("ILS", "5326888878675678", "2027", "10", "101", "3190876789", "Hadas"); 
        ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
        private const int NumThreads = 10;
        private const int NumIterations = 100;
        string productname1 = "product1";
        private ClientManager clientManager;
        string sellmethod = "RegularSell";
        string desc = "nice";
        int productCounter = 0;
        int storeId = 1;
        int userId2;

        [TestInitialize]
        public void SetUp()
        {
            Task.Run(async () => await AsyncSetUp()).GetAwaiter().GetResult();
        }

         private async Task AsyncSetUp()
        {
            DBcontext.GetInstance().Dispose();
            context = DBcontext.GetInstance();
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
            MarketManagerFacade MMF = marketManagerFacade = MarketManagerFacade.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            await c.Register(username1, "12345", "nofar@gmail.com", 19);
            token1 = (await c.LoginClient(username1, "12345")).Value;
            int storeId= await MMF.CreateStore(token1, "shop1", "shop@gmail.com", "0502552798");
            _owner = CM.GetClientByIdentifier(token1);
            _Store = await MMF.GetStore(storeId);
            await _Store.AddProduct(username1, "Brush", "RegularSell" , "Brush", 4784, "hair", 21, false);
            _p1 = _Store.Products.ToList().Find((p) => p.ProductId == 11);
            await c.Register( username2, "54321", "nofar@gmail.com", 18);
            token2 = (await c.LoginClient(username2, "54321")).Value;
        }

         [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
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
            MarketManagerFacade.Dispose();
        }

        [TestMethod()]
        public async void AddProductSuccess()
        {
            
            await _Store.AddProduct(username1, "Shampo", "RegularSell" , "Shampo", 4784, "hair", 21, false);
            Assert.IsTrue(_Store.Products.ToList().Find((p) => p.Name == "Shampo") != null);
        }

        [TestMethod()]
        public async Task AddProductFailHasNoPermissions()
        {
           await Assert.ThrowsExceptionAsync<Exception>(async () => 
            {
                await _Store.AddProduct(username2, "Shampo", "RegularSell", "Shampo", 4784, "hair", 21, false);
            });
        }

        [TestMethod()]
        public async Task AddProductFailUserNotExist()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.AddProduct(username3,"Shampo", "RegularSell" , "Shampo", 4784, "hair", 21, false));
        }

        [TestMethod()]
        public async Task RemoveProductSuccess()
        {
            await _Store.AddProduct(username1, "Shampo", "RegularSell" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            await _Store.RemoveProduct(username1, p1.ProductId);
            Assert.IsTrue(!_Store.Products.Contains(p1));
        }

        [TestMethod()]
        public async Task RemoveProductFailNOPrermissions()
        {
           await _Store.AddProduct(username1, "Shampo", "RegularSell" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.RemoveProduct(username3, p1.ProductId));
        }

        [TestMethod()]
        public async Task RemoveProductFailUserNotExist()
        {
           await _Store.AddProduct(username1, "Shampo", "RegularSell" , "Shampo", 4784, "hair", 21, false);
            Product p1 = _Store.Products.ToList().Find((p) => p.Name == "Shampo");
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.RemoveProduct(username3, p1.ProductId));
        }

        [TestMethod()]
        public async Task OpenStoreSuccess()
        {
            _Store.Active = false;
            await _Store.OpenStore(username1);
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public async Task OpenStoreFailUserNotExist()
        {
            _Store.Active = false;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.OpenStore(username3));
            Assert.IsFalse(_Store.Active);
        }

        [TestMethod()]
        public async Task OpenStoreFailUserHasNoPermissions()
        {
            _Store.Active = false;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.OpenStore(username2));
            Assert.IsFalse(_Store.Active);
        }
        [TestMethod()]
        public async Task OpenStoreFailStoreIsOpen()
        {
            _Store.Active = true;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.OpenStore(username1));
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public async Task closeStoreSuccess()
        {
            _Store.Active = true;
            await _Store.CloseStore(username1);
            Assert.IsFalse(_Store.Active);
        }

        [TestMethod()]
        public async Task CloseStoreFailUserNotExist()
        {
            _Store.Active = true;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.CloseStore(username3));
            Assert.IsTrue(_Store.Active);
        }

        [TestMethod()]
        public async Task CloseStoreFailUserHasNoPermissions()
        {
            _Store.Active = true;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.CloseStore(username2));
            Assert.IsTrue(_Store.Active);
        }
        [TestMethod()]
        public async Task CloseStoreFailStoreIsClose()
        {
            _Store.Active = false;
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.CloseStore(username1));
            Assert.IsFalse(_Store.Active);
        }

         public async Task UpdateProductPriceSuccess()
        {
            await _Store.UpdateProductPrice(username1, _p1.ProductId, 45555);
            Assert.AreEqual(45555, _p1.Price,
            $"Expected price of 45555 but got {_p1.Price}");
        }

        [TestMethod()]
        public async Task UpdateProductPriceFailUserNotExist()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.UpdateProductPrice(username3, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public async Task UpdateProductPriceFailUserHasNotPermissions()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.UpdateProductPrice(username2, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public async Task UpdateProductQuantitySuccess()
        {
            await _Store.UpdateProductQuantity(username1, _p1.ProductId, 45555);
            Assert.AreEqual(45555, _p1.Quantity,
            $"Expected quantity of 45555 but got {_p1.Quantity}");
        }

        [TestMethod()]
        public async Task UpdateProductQuantityFailUserNotExist()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.UpdateProductQuantity(username3, _p1.ProductId, 45555));
        }

         [TestMethod()]
        public async Task UpdateProductQuantityFailUserHasNotPermissions()
        {
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.UpdateProductQuantity(username2, _p1.ProductId, 45555));
        }

        [TestMethod()]
        public async Task AddStaffMemberSuccess()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, username2);
            await _Store.AddStaffMember(username2, role, username1);
            Assert.IsTrue(_Store.roles.ContainsKey(username1));

        }

        [TestMethod()]
        public async Task AddStaffMemberFailUserNotExist()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, username2);
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.AddStaffMember(username2, role, username3));
            Assert.IsFalse(_Store.roles.ContainsKey(username2));

        }

        [TestMethod()]
        public async Task AddStaffMemberFailUserHasNoPermissions()
        {
            Role role = new Role(new StoreManagerRole(RoleName.Manager), (Member)_owner, _Store._storeId, username2);
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.AddStaffMember(username2, role, username2));
            Assert.IsFalse(_Store.roles.ContainsKey(username2));
        }

        [TestMethod()]
        public async Task AddStaffMemberFailAnotherFounder()
        {
            Role role = new Role(new Founder(RoleName.Founder), (Member)_owner, _Store._storeId, username2);
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _Store.AddStaffMember(username2, role, username1));
            Assert.IsFalse(_Store.roles.ContainsKey(username2));

        }

        [TestMethod()]
        public async Task PurchaseBasketSuccess()
        {
            Basket basket = new Basket(1, _Store._storeId);
            basket._cartId = (await marketManagerFacade.GetMember(username1)).Cart._shoppingCartId;
            BasketDTO basketDTO = new BasketDTO(basket);
            context.Baskets.Add(basketDTO);
            await basket.addToBasket(11, 10);
            Purchase purchase = await _Store.PurchaseBasket(username2,basket);
            Assert.IsTrue(_Store._history._purchases.Contains(purchase));
            Product product = _Store.GetProduct(11);
            Assert.AreEqual(11, product._quantity,
            $"Expected product quantity of 11 but got {product._quantity}");
        }

        [TestMethod()]
        public async Task PurchaseBasketFailStoreIsClose()
        {
            Basket basket = new Basket(13, _Store._storeId);
            basket._cartId = (await marketManagerFacade.GetMember(username1)).Cart._shoppingCartId;
            BasketDTO basketDTO = new BasketDTO(basket);
            context.Baskets.Add(basketDTO);
            await basket.addToBasket(11, 10);
            _Store._active=false;
            await Assert.ThrowsExceptionAsync<Exception>(() => _Store.PurchaseBasket(username2,basket));
            Product product = _Store.GetProduct(11);
            Assert.AreEqual(21, product._quantity,
            $"Expected product quantity of 21 but got {product._quantity}");
            
        }

         [TestMethod()]
        public async Task PurchaseBasketFailnotEnoughQuantity()
        {
            Basket basket = new Basket(13, _Store._storeId);
            basket._cartId = (await marketManagerFacade.GetMember(username1)).Cart._shoppingCartId;
            BasketDTO basketDTO = new BasketDTO(basket);
            context.Baskets.Add(basketDTO);
            await basket.addToBasket(11, 40);
            await Assert.ThrowsExceptionAsync<Exception>(() => _Store.PurchaseBasket(username2,basket));
            Product product = _Store.GetProduct(11);
            Assert.AreEqual(21, product._quantity,
            $"Expected product quantity of 21 but got {product._quantity}");
            
        }



 




    }
}