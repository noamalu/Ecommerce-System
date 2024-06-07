using Microsoft.AspNetCore.Mvc;
using WebSocketSharp.Server;
using Microsoft.AspNetCore.Session;
using System.Xml.Schema;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using MarketBackend.Services;
using System.ComponentModel.DataAnnotations;
using EcommerceAPI.Models.Dtos;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services.Models;
using MarketBackend.Domain.Shipping;
using MarketBackend.Domain.Payment;

namespace EcommerceAPI.Controllers
{

    [ApiController]
    [Route("api/Client")]
    public class ClientController : ControllerBase
    {
        private WebSocketServer _alertServer;
        private WebSocketServer _logServer;
        private IClientService _clientService;

        private static Dictionary<string, IList<string>> _clientPendingAlerts = new();
        private static Dictionary<int, string> _clientTokenIdxAlertPath = new();
        public ClientController(WebSocketServer alerts, WebSocketServer logs, IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem)
        {
            this._clientService = ClientService.GetInstance(shippingSystemFacade, paymentSystem);
            this._alertServer = alerts;
            NotificationManager.GetInstance(alerts);
            this._logServer = logs;
        }
        private class NotificationsService : WebSocketBehavior
        {

        }
        public class logsService : WebSocketBehavior
        {

        }        
        
        [HttpPost]
        [Route("Guest/Login")]
        public async Task<ActionResult<ServerResponse<string>>> Login([Required][FromQuery]int tokenId, [FromBody] ClientDto client)
        {
            string relativePath = $"/{client.Username}-alerts";
            try
            {
                if (_alertServer.WebSocketServices[relativePath] == null)
                {
                    _alertServer.AddWebSocketService<NotificationsService>(relativePath);

                }
            }
            catch (Exception ex)
            {
                var loginResponse = new ServerResponse<string>
                {
                    ErrorMessage = ex.Message,
                };
                return BadRequest(loginResponse);
            }
            
            Response response = await Task.Run(() => _clientService.LoginClient(tokenId, client.Username, client.Password));
            if (response.ErrorOccured)
            {
                _alertServer.RemoveWebSocketService(relativePath);
                var loginResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(loginResponse);
            }
            else
            {
                _clientTokenIdxAlertPath.Add(tokenId, relativePath);
                var createShopResponse = new ServerResponse<string>
                {
                    Value = tokenId.ToString(),
                };
                return Ok(createShopResponse);
            }
        }
        private void AddPendingAlert(string username, IList<string> messages)
        {
            _clientPendingAlerts[username] = messages;
        }


        [HttpPost]
        [Route("Member/Logout")]
        public async Task<ActionResult<ServerResponse<string>>> Logout([Required][FromQuery]int tokenId, [FromBody] ClientDto client)
        {
            Response response = await Task.Run(() => _clientService.LoginClient(tokenId, client.Username, client.Password));
            ServerResponse<string> logoutResponse;
            if (response.ErrorOccured)
            {
                logoutResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(logoutResponse);
            }
            if (_clientTokenIdxAlertPath.ContainsKey(tokenId))
            {
                _alertServer.RemoveWebSocketService(_clientTokenIdxAlertPath[tokenId]);
                _clientTokenIdxAlertPath.Remove(tokenId);
            }
            logoutResponse = new ServerResponse<string>
            {
                Value = "logout success",
            };
            return Ok(logoutResponse);
        }

        [HttpPost]
        [Route("Guest")]
        public async Task<ActionResult<ServerResponse<string>>> EnterAsGuest([Required][FromQuery]int tokenId)
        {
            string session = HttpContext.Session.Id;
            Response response = await Task.Run(() => _clientService.EnterAsGuest(tokenId));
            if (response.ErrorOccured)
            {
                var enterAsGuestResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(enterAsGuestResponse);
            }
            else
            {
                var enterAsGuestResponse = new ServerResponse<string>
                {
                    Value = session,
                };
                return Ok(enterAsGuestResponse);
            }
        }

        [HttpPost]
        [Route("Guest/Register")]
        public async Task<ActionResult<ServerResponse<string>>> Register([Required][FromQuery]int tokenId, [FromBody] ExtendedClientDto client)
        {
            Response response = await Task.Run(() => _clientService.Register(tokenId, client.Username, client.Password, client.Email, client.Age));
            if (response.ErrorOccured)
            {
                var registerResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(registerResponse);
            }
            else
            {
                var registerResponse = new ServerResponse<string>
                {
                    Value = tokenId.ToString(),
                };
                return Ok(registerResponse);
            }

        }                                            

        [HttpPut] //client controller
        [Route("Cart")]
        public async Task<ActionResult<ServerResponse<string>>> UpdateCart([Required][FromQuery]int tokenId, [FromBody] ProductDto product)
        {
            if(!product.IsValidForCart()) return BadRequest("product must contain id, store id and product name");

            Response response = product.Quantity > 0 ? 
                await Task.Run(() => _clientService.AddToCart(tokenId, product.StoreId, (int)product.Id, product.Quantity)) : 
                await Task.Run(() => _clientService.RemoveFromCart(tokenId, product.StoreId, (int)product.Id, Math.Abs(product.Quantity)));
            if (response.ErrorOccured)
            {
                var addToCartResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(addToCartResponse);
            }
            else
            {
                var addToCartResponse = new ServerResponse<string>
                {
                    Value = "Cart update success",
                };
                return Ok(addToCartResponse);
            }
        }

        [HttpGet]
        [Route("Cart")]
        public ActionResult<Response<ShoppingCartResultDto>> GetShoppingCartInfo([Required][FromQuery]int tokenId)
        {
            Response<ShoppingCartResultDto> response = _clientService.ViewCart(tokenId);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<ShoppingCartResultDto>.OkResponse(response.Value));
            }
        }                

        [HttpGet]
        [Route("Member/PurchaseHistory")]
        public ActionResult<Response<List<ShoppingCartResultDto>>> GetMemberPurchaseHistory([Required][FromQuery]int tokenId)
        {
            Response<List<ShoppingCartResultDto>> response = _clientService.GetPurchaseHistory(tokenId);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<List<ShoppingCartResultDto>>.OkResponse(response.Value));
            }
        }   

        [HttpPost]
        [Route("Client/CreateStore")]
        public async Task<ObjectResult> CreateStore([Required][FromQuery]int tokenId, [Required][FromBody] StoreDto storeInfo)
        {
            Response response = await Task.Run(() => _clientService.CreateStore(tokenId, storeInfo.Name, storeInfo.Email, storeInfo.PhoneNum));
            if (response.ErrorOccured)
            {
                var openShopResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(openShopResponse);
            }
            else
            {
                var openShopResponse = new ServerResponse<string>
                {
                    Value = "create-shop-success",
                };
                return Ok(openShopResponse);
            }
        }             
    }
}
