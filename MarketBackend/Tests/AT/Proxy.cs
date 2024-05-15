using MarketBackend.Services;

namespace MarketBackend.Tests.AT
{
    public class Proxy
    {
        MarketService marketService;
        ClientService clientService;

        int userId = 1;

        public Proxy(){
            marketService = MarketService.GetInstance();
            clientService = ClientService.GetInstance();
        }

        public void Dispose(){
            marketService.Dispose();
            clientService.Dispose();
        }

        public int GetUserId(){
            string userIds = userId.ToString();
            userId = int.Parse(userIds) + 1;
            return userId;
        }

        public bool Login(int userId, string userName, string password){
            Response res = clientService.LoginClient(userId, userName, password);
            return !res.ErrorOccured;
        }

        public bool EnterAsGuest(int userId){
            Response res = clientService.EnterAsGuest(userId);
            return !res.ErrorOccured;
        }

        public bool Register(string userName, string password){
            Response res = clientService.Register(userName, password);
            return !res.ErrorOccured;
        }

        public bool LogOut(int userId){
            Response res = clientService.LogoutClient(userId);
            return !res.ErrorOccured;
        }

        
    }
}