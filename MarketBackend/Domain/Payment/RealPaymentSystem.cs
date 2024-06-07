using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Payment
{
    public class RealPaymentSystem : IPaymentSystemFacade
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public RealPaymentSystem()
        {}
        public RealPaymentSystem(HttpClient httpClient, string url)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public virtual int Pay(PaymentDetails cardDetails, double totalAmount)
        {
            if (_url == null)
                throw new NotImplementedException();

            var parameters = new Dictionary<string, string>
            {
                { "action_type", "pay" },
                {"payment_id", cardDetails.PaymentID.ToString()},
                { "card_number", cardDetails.CardNumber },
                { "month", cardDetails.ExprMonth },
                { "year", cardDetails.ExprYear },
                { "holder", cardDetails.HolderName },
                { "ccv", cardDetails.Cvv },
                { "id", cardDetails.HolderID },
                {"amount", totalAmount.ToString()}
            };

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new FormUrlEncodedContent(parameters)
            })
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (responseBody.Equals("OK"))
                    {
                        if (response.Headers.TryGetValues("TransactionId", out var transactionIdValues))
                        {
                            return Convert.ToInt32(transactionIdValues.FirstOrDefault());
                        }
                    }
                }
            }
            return -1;
        }

        public int CancelPayment(int paymentID)
        {
            if (_url == null)
                throw new NotImplementedException();

            var parameters = new Dictionary<string, string>
            {
                { "action_type", "cancel_payment" },
                { "transaction_ID", paymentID.ToString() }
            };

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new FormUrlEncodedContent(parameters)
            })
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (responseBody.Equals("OK"))
                    {
                        return 1;
                    }
                }
            }
            return -1;
        }

        public bool Connect()
        {
            if (_url == null)
                throw new NotImplementedException();

            var parameters = new Dictionary<string, string>
            {
                { "action_type", "connect" }
            };

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new FormUrlEncodedContent(parameters)
            })
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
