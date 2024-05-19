using System.Text;
using MarketBackend.Domain.Models;
namespace MarketBackend.Domain.Market_Client
{
    public class Purchase{
        private int _purchaseId;
        private int _storeId;
        private int _clientId;
        private Basket _basket;
        double _price;

        public int PurchaseId { get => _purchaseId; }
        public int StoreId { get => _storeId; }
        public int ClientId { get => _clientId; }
        public double Price { get => _price; }
        public Basket Basket { get => _basket; }
        public Purchase(int id, int storeId, int clientId, Basket basket, double basketPrice)
        {
            _purchaseId = id;
            _storeId = storeId;
            _clientId = clientId;
            _basket = basket;
            _price = basketPrice;
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---------------------------");
            sb.AppendLine(string.Format("Purchase Number: %d", _storeId));
            sb.AppendLine(string.Format("Buyer ID: %d", _clientId));
            sb.AppendLine(string.Format("Shop ID: %d", _storeId));
            sb.AppendLine(string.Format("Basket: %s", _basket.GetInfo()));
            sb.AppendLine("---------------------------");
            return sb.ToString();
        }
    }
}