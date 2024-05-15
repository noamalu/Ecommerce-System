using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;

namespace MarketBackend.Domain.Market_Client
{
    public class Product
    {
        private int _id;
        private int _shopId;
        private string _name;
        private double _price;
        private int _quantity;
        private string _category;
        private ConcurrentBag<string> _keywords;
        private string _description;
        private ISellMethod _sellMethod;


        public int Id { get => _id; }
        public int ShopId { get => _shopId; }
        public string Name { get => _name; set => _name = value; }
        public double Price { get => _price; set => _price = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public string Category { get => _category; set => _category = value; }
        public string Description { get => _description; set => _description = value; }
        public ConcurrentBag<string> Keywords { get => _keywords; set => _keywords = value; }
        public ISellMethod SellMethod { get => _sellMethod; set => _sellMethod = value; }


        public Product(int id, int shopId, string name,string sellMethod, string description, double price, string category, int quantity)
        {
            _id = id;
            _shopId = shopId;
            _name = name;
            _description = description;
            _price = price;
            _quantity = quantity;
            _category = category;
            _keywords = new ConcurrentBag<string>();
            _sellMethod = createSellMethod(sellMethod);
        }


        public ISellMethod createSellMethod(string sellMethod)
        {
            switch (sellMethod)
            {
                case "RegularSell":
                    return new RegularSell();
                case "BidSell":
                    return new BidSell();
                case "AuctionSell":
                    return new AuctionSell();
                case "LotterySell":
                    return new LotterySell();    
                default:
                    throw new ArgumentException("Invalid sell method");
            }
        }

        public bool ContainKeyword(string keyWord)
        {
            return _keywords.ToList().Find((key) => key.ToLower().Equals(keyWord.ToLower())) != null;
        }

        public void AddKeyword(string keyWord)
        {
            _keywords.Add(keyWord);
        }

        public void RemoveKeyword(string keyWord)
        {
           if(!_keywords.TryTake(out keyWord))
            {
                throw new ArgumentException("Keyword not found");
            }
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---------------------------");
            sb.AppendLine(string.Format("Product ID: %d", _id));
            sb.AppendLine(string.Format("Shop ID: %d", _shopId));
            sb.AppendLine(string.Format("Product Description: %s", _description));
            sb.AppendLine(string.Format("Quantity in stock: %d", _quantity));
            sb.AppendLine(string.Format("Catagroy: %s", _category.ToString()));
            sb.AppendLine("---------------------------");
            return sb.ToString();
        }

        public bool HasCategory(string category)
        {
            return _category == category;
        }

        public override string ToString()
        {
            return _name;
        }

        public void updatePrice(double newPrice)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException("Price can't be negative");
            }
            _price = newPrice;
        }

        public void updateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentException("Quantity can't be negative");
            }
            _quantity = newQuantity;
        }
    }
    
}