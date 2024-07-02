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
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _url;

        
        public RealShippingSystem(string URL)
        {
            _url = URL;
        }
        public virtual int OrderShippment(ShippingDetails details)
        {
                if (!Connect() || details == null)
                    return -1;
                var parameters = new Dictionary<string, string>
                {
                    { "action_type", "supply" },
                    { "name", details.Name },
                    { "address", details.Address },
                    { "city", details.City },
                    { "country", details.Country},
                    { "zip", details.Zipcode }
        
                };
                
                var supplyContent = new FormUrlEncodedContent(parameters); //wrap the request as post content
                var response = _httpClient.PostAsync(_url, supplyContent).Result; //send the post request to the web service
                if (response.IsSuccessStatusCode){
                    string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                    if (!responseContent.Equals("-1"))
                        return int.Parse(responseContent);
                    }
                return -1;
            
            
        }
        

        public int CancelShippment(int orderID)
        {
           
            if (!Connect() || orderID < 10000 || orderID > 1000000)
                return -1;


            var parameters = new Dictionary<string, string>
            {
                { "action_type", "cancel_supply" },
                {"transaction_id", orderID.ToString()}
            };
            
            var cancelSupplyContent = new FormUrlEncodedContent(parameters); //wrap the request as post content
            var response = _httpClient.PostAsync(_url, cancelSupplyContent).Result; //send the post request to the web service
            if (response.IsSuccessStatusCode){
                string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                if (responseContent.Equals("1"))
                    return int.Parse(responseContent);
            }
            return -1;
            
        
        }

        public bool Connect()
        {
            var content = new Dictionary<string, string>
            {
                { "action_type", "handshake" }
            };
        
            var handshakeContect = new FormUrlEncodedContent(content); //wrap the request as post content
            var response = _httpClient.PostAsync(_url, handshakeContect).Result; //send the post request to the web service
            if (response.IsSuccessStatusCode) 
            {
                string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                if (responseContent.Equals("OK"))
                    return true;
            }
            return false;
    
        }
        

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}

     
