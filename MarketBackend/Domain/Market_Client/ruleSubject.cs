namespace MarketBackend.Domain.Market_Client{
    public class RuleSubject{
        private Product _product;
        private string _category;

        public Product Product { get => _product; set => _product = value; }
        public string Category { get => _category; set => _category = value; }
        public RuleSubject() {
            _category = "All";
        }
        public RuleSubject(Product product)
        {
            _product = product;
            _category = product.Category;
        }
        public RuleSubject(string category)
        {
            _category = category;
        }
        public bool IsProduct()
        {
            return _product != null;
        }
        public string GetInfo()
        {
            if (IsProduct()) { return _product.ToString(); }
            else { return _category.ToString(); }
        }
    }
}