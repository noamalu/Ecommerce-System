using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketBackend.Domain.Payment
{
    public class RealPaymentSystem : IPaymentSystemFacade
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _url;

        public RealPaymentSystem(string URL)
        {
            _url = URL;
        }

        public virtual int Pay(PaymentDetails cardDetails, double totalAmount)
        {
            if (!Connect() || totalAmount <= 0)
                return -1;
            
            var parameters = new Dictionary<string, string>
            {
                { "action_type", "pay" },
                {"amount", totalAmount.ToString()},
                {"currency", cardDetails.Currency},
                { "card_number", cardDetails.CardNumber },
                { "month", cardDetails.ExprMonth },
                { "year", cardDetails.ExprYear },
                { "holder", cardDetails.HolderName },
                { "cvv", cardDetails.Cvv },
                { "id", cardDetails.HolderID }
        
            };
            var payContect = new FormUrlEncodedContent(parameters); //wrap the request as post content
            var response = _httpClient.PostAsync(_url, payContect).Result; //send the post request to the web service
            if (response.IsSuccessStatusCode){ 
                string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                if (!responseContent.Equals("-1"))
                    return int.Parse(responseContent);
                }

            return -1; 
        
        }


        public int CancelPayment(int transactionId)
        {
            if (!Connect() || transactionId < 10000 || transactionId > 1000000)
            {
                return -1;
            }

            var parameters = new Dictionary<string, string>
            {
                { "action_type", "cancel_pay" },
                { "transaction_id", transactionId.ToString() }
            };

    
            var cancelPayContect = new FormUrlEncodedContent(parameters); //wrap the request as post content
            var response = _httpClient.PostAsync(_url, cancelPayContect).Result; //send the post request to the web service
            if (response.IsSuccessStatusCode){
                string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                if (responseContent.Equals("1")) //success
                {
                    return 1;
                }
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
            if(response.IsSuccessStatusCode){
                string responseContent = response.Content.ReadAsStringAsync().Result; //get the response from the web service
                if (responseContent.Equals("OK"))
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
