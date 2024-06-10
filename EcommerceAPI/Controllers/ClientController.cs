﻿using Microsoft.AspNetCore.Mvc;
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
        private static Dictionary<string, string> _alertPathByclientIdentifier = new();
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
        public async Task<ActionResult<ServerResponse<string>>> Login([FromBody] ClientDto client)
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
            
            Response<string> response = await Task.Run(() => _clientService.LoginClient(client.Username, client.Password));
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
                _alertPathByclientIdentifier.Add(response.Value, relativePath);
                var createShopResponse = new ServerResponse<string>
                {
                    Value = response.Value,
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
        public async Task<ActionResult<ServerResponse<string>>> Logout([Required][FromQuery]string identifier)
        {
            Response response = await Task.Run(() => _clientService.LogoutClient(identifier));
            ServerResponse<string> logoutResponse;
            if (response.ErrorOccured)
            {
                logoutResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(logoutResponse);
            }
            if (_alertPathByclientIdentifier.ContainsKey(identifier))
            {
                _alertServer.RemoveWebSocketService(_alertPathByclientIdentifier[identifier]);
                _alertPathByclientIdentifier.Remove(identifier);
            }
            logoutResponse = new ServerResponse<string>
            {
                Value = "logout success",
            };
            return Ok(logoutResponse);
        }

        [HttpPost]
        [Route("Guest")]
        public async Task<ActionResult<ServerResponse<string>>> EnterAsGuest([Required][FromQuery]string identifier)
        {
            string session = HttpContext.Session.Id;
            Response response = await Task.Run(() => _clientService.EnterAsGuest(identifier));
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
        public async Task<ActionResult<ServerResponse<string>>> Register([FromBody] ExtendedClientDto client)
        {
            Response response = await Task.Run(() => _clientService.Register(client.Username, client.Password, client.Email, client.Age));
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
                    Value = client.Username,
                };
                return Ok(registerResponse);
            }

        }                                            

        [HttpPut] //client controller
        [Route("Cart")]
        public async Task<ActionResult<ServerResponse<string>>> UpdateCart([Required][FromQuery]string identifier, [FromBody] ProductDto product)
        {
            if(!product.IsValidForCart()) return BadRequest("product must contain id, store id and product name");

            Response response = product.Quantity > 0 ? 
                await Task.Run(() => _clientService.AddToCart(identifier, product.StoreId, (int)product.Id, product.Quantity)) : 
                await Task.Run(() => _clientService.RemoveFromCart(identifier, product.StoreId, (int)product.Id, Math.Abs(product.Quantity)));
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
        public ActionResult<Response<ShoppingCartResultDto>> GetShoppingCartInfo([Required][FromQuery]string identifier)
        {
            Response<ShoppingCartResultDto> response = _clientService.ViewCart(identifier);
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
        public ActionResult<Response<List<ShoppingCartResultDto>>> GetMemberPurchaseHistory([Required][FromQuery]string identifier)
        {
            Response<List<ShoppingCartResultDto>> response = _clientService.GetPurchaseHistoryByClient(identifier);
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
        public async Task<ObjectResult> CreateStore([Required][FromQuery]string identifier, [Required][FromBody] StoreDto storeInfo)
        {
            Response response = await Task.Run(() => _clientService.CreateStore(identifier, storeInfo.Name, storeInfo.Email, storeInfo.PhoneNum));
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
