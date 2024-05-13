using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace MarketBackend.Domain.Shipping
{
    public class RealShippingSystem : IShippingSystemFacade
    {
        private readonly string? url = null; //TODO: remove ? and change url
        HttpClient httpClient = new HttpClient();
        HttpRequestMessage httpRequest;

         public RealShippingSystem()
        {
            httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        }


       public bool Connect()
        {
            if (url == null)
                throw new NotImplementedException();

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    {"action_type", "connect"}
                };
                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(parameters)).Result;
            

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
        public void OrderShippment(ShippingDeatails details)
        {
          if (url == null)
                throw new NotImplementedException();

            using (HttpClient client = new HttpClient())
            {

                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "shipping" },
                    { "name", details.Name },
                    { "address", details.Address },
                    { "city", details.City },
                    { "country", details.Country},
                    { "zipcode", details.Zipcode }
        
                };

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(parameters)).Result;

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

        public int CancelShippment(int orderID)
        {
             if (url == null)
                throw new NotImplementedException();

            using (HttpClient client = new HttpClient())
            {

                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "cancel_shipping" },
                    {"transaction_ID", orderID.ToString()}
                };

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(parameters)).Result;

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


        }
    }

     
