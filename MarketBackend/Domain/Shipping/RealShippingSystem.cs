using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace MarketBackend.Domain.Shipping
{
    public class RealShippingSystem : IShippingSystemFacade
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public RealShippingSystem()
        {}
        public RealShippingSystem(HttpClient httpClient, string url)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _url = url ?? throw new ArgumentNullException(nameof(url));
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
        public virtual int OrderShippment(ShippingDetails details)
        {
          if (_url == null)
                throw new NotImplementedException();

            

                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "shipping" },
                    {"shippind_id", details.ShippingID.ToString()},
                    { "name", details.Name },
                    { "address", details.Address },
                    { "city", details.City },
                    { "country", details.Country},
                    { "zipcode", details.Zipcode }
        
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
        

        public int CancelShippment(int orderID)
        {
             if (_url == null)
                throw new NotImplementedException();

                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "cancel_shipping" },
                    {"transaction_ID", orderID.ToString()}
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

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
    }

     
