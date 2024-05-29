using System.Text.Json;
using WebSocketSharp.Server;

namespace MarketBackend.Domain.Market_Client
{
    public class NotificationManager
    {
        private static NotificationManager _alertsManager = null;
        private static object _lock = new object();
        private WebSocketServer _alertsServer;

        private NotificationManager(WebSocketServer alertsServer) 
        {
            _alertsServer = alertsServer;
        }
        private NotificationManager() 
        {
        }

        public static NotificationManager GetInstance(WebSocketServer alertsServer)
        {            
            if (_alertsManager is not null){
                _alertsManager._alertsServer = alertsServer;
                return _alertsManager;
            }
            lock (_lock)
            {
                _alertsManager ??= new NotificationManager(alertsServer);
            }
            return _alertsManager;
        }

        public static NotificationManager GetInstance()
        {            
            if (_alertsManager is not null)
                return _alertsManager;
            lock (_lock)
            {
                _alertsManager ??= new NotificationManager();
            }
            return _alertsManager;
        }

        public void SendNotification(string message, string username)
        {
            var relativePath = $"/{username}-alerts";

            if (_alertsServer is null || _alertsServer.WebSocketServices[relativePath] is null)
                return;

            var webSocketService = _alertsServer.WebSocketServices[relativePath];
            if (webSocketService is null || webSocketService.Sessions.Count <= 0)
                return;                
            
            var json = JsonSerializer.Serialize(new { message });

            lock(_lock)
            {
                foreach (var session in webSocketService.Sessions.Sessions)
                {
                    try{
                        var webSocket = session.Context.WebSocket;
                        if (webSocket?.IsAlive ?? false)
                            webSocket.Send(json);
                    }catch(Exception)
                    {
                        throw;
                    }
                }                
            }
            
        }
    }
}
