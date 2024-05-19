using System;
using System.Collections;
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

        HttpRequestMessage httpRequest;

        

        public RealPaymentSystem(HttpClient httpClient, string url)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _url = url ?? throw new ArgumentNullException(nameof(url));
        }
        

        public int Pay(PaymentDetails cardDetails, double totalAmount)
        {
            if (_url == null)
                throw new NotImplementedException();

            using (HttpClient client = _httpClient)
            {

                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "pay" },
                    { "card_number", cardDetails.CardNumber },
                    { "month", cardDetails.ExprMonth },
                    { "year", cardDetails.ExprYear },
                    { "holder", cardDetails.HolderName },
                    { "ccv", cardDetails.Cvv },
                    { "id", cardDetails.HolderID }
                };

                HttpResponseMessage response = client.PostAsync(_url, new FormUrlEncodedContent(parameters)).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    if (responseBody.Equals("OK"))
                    {    
                        return Convert.ToInt32(response.Headers.GetValues("TransactionId").FirstOrDefault());
                    }
                    
                }
            }
            return -1;
        }

        public int CancelPayment(int paymentID)
        {
             if (_url == null)
                throw new NotImplementedException();

            try
            {
            using (HttpClient client = _httpClient)
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        {"action_type", "cancel_payment"},
                        {"transaction_ID", paymentID.ToString()}
                    };
                    HttpResponseMessage response = client.PostAsync(_url, new FormUrlEncodedContent(parameters)).Result;
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }

        public bool Connect()
        {
            if (_url == null)
                throw new NotImplementedException();

            using (HttpClient client = _httpClient)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    {"action_type", "connect"}
                };
                HttpResponseMessage response = client.PostAsync(_url, new FormUrlEncodedContent(parameters)).Result;
            

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }



}