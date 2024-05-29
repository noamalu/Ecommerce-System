namespace MarketBackend.Domain.Market_Client
{
public class DiscountPolicy : IPolicy
    {
        private double _precentage;

        public double Precentage { get => _precentage; set => _precentage = value; }
    }
}