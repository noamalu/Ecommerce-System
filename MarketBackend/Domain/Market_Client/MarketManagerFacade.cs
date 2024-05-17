using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;
using Microsoft.Extensions.Logging;


namespace MarketBackend.Domain.Market_Client
{
    public class MarketManagerFacade : IMarketManagerFacade
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<MarketManagerFacade> _logger;
        public MarketManagerFacade(ILogger<MarketManagerFacade> logger, IClientRepository clientRepository){
            _clientRepository = clientRepository;
            _logger = logger;
        }
        public void AddManger(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddOwner(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddPermission(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        public void AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            Client client = _clientRepository.GetById(clientId);
            client.addToCart(storeId ,productId, quantity);
            _logger.LogInformation($"Product id={productId} were added to client id={clientId} cart, to storeId={storeId} basket.!");
        }

        public void BrowseGuest()
        {
            throw new NotImplementedException();
        }

        public void CloseStore(int storeId)
        {
            throw new NotImplementedException();
        }

        public void CreateStore(int id)
        {
            throw new NotImplementedException();
        }

        public void EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public void EnterAsGuest()
        {
            throw new NotImplementedException();
        }

        public void ExitGuest()
        {
            throw new NotImplementedException();
        }

        public void GetFounder()
        {
            throw new NotImplementedException();
        }

        public void GetMangers()
        {
            throw new NotImplementedException();
        }

        public void GetOwners()
        {
            throw new NotImplementedException();
        }

        public string GetProductInfo()
        {
            throw new NotImplementedException();
        }

        public void GetPurchaseHistoryByClient(int id)
        {
            throw new NotImplementedException();
        }

        public void GetPurchaseHistoryByStore(int id)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission()
        {
            throw new NotImplementedException();
        }

        public void IsAvailable(int productId)
        {
            throw new NotImplementedException();
        }

        public void LoginClient(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void LogoutClient(int id)
        {
            throw new NotImplementedException();
        }

        public void OpenStore(int storeId)
        {
            throw new NotImplementedException();
        }

        public void PurchaseCart(int id)
        {
            throw new NotImplementedException();
        }

        public void Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        public void RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void RemoveStaffMember(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void ResToStoreManageReq()
        {
            throw new NotImplementedException();
        }

        public void ResToStoreOwnershipReq()
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByCategory()
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByKeyWords()
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByName()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductPrice(int productId, int price)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public void ViewCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}
