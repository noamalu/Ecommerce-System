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
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Models;
using MarketBackend.Domain.Shipping;

namespace EcommerceAPI.Controllers
{
    public class ServerResponse<T>
    {
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
        public static ServerResponse<T> OkResponse(T val)
        {
            var response = new ServerResponse<T>
            {
                Value = val,
            };
            return response;
        }
        public static ServerResponse<T> BadResponse(string msg)
        {
            var response = new ServerResponse<T>
            {
                ErrorMessage = msg,
            };
            return response;
        }
        
    }

    [ApiController]
    [Route("api/Market")]
    public class MarketController : ControllerBase
    {
        private WebSocketServer _alertServer;
        private WebSocketServer _logServer;
        private IMarketService _marketService;

        private static IDictionary<string, IList<string>> buyerUnsentMessages = new Dictionary<string, IList<string>>();
        private static IDictionary<string, string> buyerIdToRelativeNotificationPath = new Dictionary<string, string>();
        public MarketController(WebSocketServer alerts, WebSocketServer logs, IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem)
        {
            this._marketService = MarketService.GetInstance(shippingSystemFacade, paymentSystem);
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

        // [HttpPost]
        // [Route("Staff")]
        // public async Task<ObjectResult> Appoint([Required][FromQuery]int tokenId, [FromBody] StaffMemberDto staffMember)
        // {
        //     var role = GetRoleByName(staffMember.Role);
        //     Response<int> response = await Task.Run(() => _marketService.AddStaffMember(staffMember.StoreId, tokenId, new Role(new(role), new(), staffMember.StoreId, staffMember.Id), staffMember.Id));
            
        //     if (response.ErrorOccured)
        //     {
        //         var appointResponse = new ServerResponse<string>
        //         {
        //             ErrorMessage = response.ErrorMessage,
        //         };
        //         return BadRequest(appointResponse);
        //     }
        //     else
        //     {
        //         var appointResponse = new ServerResponse<string>
        //         {
        //             Value = "Appoint success",
        //         };
        //         return Ok(appointResponse);
        //     }
        //     return Ok(response);

        // }

        private RoleName GetRoleByName(string description)
        {
            foreach (RoleName roleName in Enum.GetValues(typeof(RoleName)))
            {
                if (roleName.GetDescription().Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return roleName;
                }
            }
            return default;
        }
        // [HttpPost]
        // [Route("remove-appoint")]
        // public async Task<ObjectResult> RemoveAppoint([Required][FromQuery]int tokenId, [FromBody] StaffMemberDto staffMember)
        // {
        //     Response response = await Task.Run(() => _marketService.RemoveStaffMember(staffMember.StoreId, tokenId, staffMember.Id));
        //     if (response.ErrorOccured)
        //     {
        //         var removeAppointResponse = new ServerResponse<string>
        //         {
        //             ErrorMessage = response.ErrorMessage,
        //         };
        //         return BadRequest(removeAppointResponse);
        //     }
        //     else
        //     {
        //         var removeAppointResponse = new ServerResponse<string>
        //         {
        //             Value = "remove Appoint success",
        //         };
        //         return Ok(removeAppointResponse);
        //     }
        // }

        // [HttpPost]
        // [Route("Store/Permision")]
        // public async Task<ObjectResult> AddPermission([FromBody] ChangePermisionRequest request)
        // {
        //     Response response = await Task.Run(() => _marketService.AddPermission(request.SessionId, request.AppointeeUserName, request.ShopId, request.Permission));
        //     if (response.ErrorOccured)
        //     {
        //         var changePermissionResponse = new ServerResponse<string>
        //         {
        //             ErrorMessage = response.ErrorMessage,
        //         };
        //         return BadRequest(changePermissionResponse);
        //     }
        //     else
        //     {
        //         var changePermissionResponse = new ServerResponse<string>
        //         {
        //             Value = "change-permission-success",
        //         };
        //         return Ok(changePermissionResponse);
        //     }
        // }

        [HttpPost]
        [Route("Store/{storeId}/Close")]
        public async Task<ObjectResult> CloseStore([Required][FromQuery]int tokenId, [FromRoute] int storeId)
        {
            Response response = await Task.Run(() => _marketService.CloseStore(tokenId, storeId));
            if (response.ErrorOccured)
            {
                var closeShopResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(closeShopResponse);
            }
            else
            {
                var closeShopResponse = new ServerResponse<string>
                {
                    Value = "close store success",
                };
                return Ok(closeShopResponse);
            }
        }        

        [HttpPost]
        [Route("Store/{storeId}/Open")]
        public async Task<ObjectResult> OpenStore([Required][FromQuery]int tokenId, [FromRoute] int storeId)
        {
            Response response = await Task.Run(() => _marketService.OpenStore(tokenId, storeId));
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
                    Value = "open-shop-success",
                };
                return Ok(openShopResponse);
            }
        }

        [HttpPost]
        [Route("Purchase")]
        public async Task<ObjectResult> PurchaseCart([Required][FromQuery]int tokenId, [FromBody] PurchaseDto purchaseInfo)
        {
            if(!purchaseInfo.IsValid()) return BadRequest("all fileds are required");
            var shippingDetails = purchaseInfo.ShippingInfo();
            var paymentDetails = purchaseInfo.PaymentInfo();
            Response response = await Task.Run(() => _marketService.PurchaseCart(tokenId, paymentDetails, shippingDetails));
            if (response.ErrorOccured)
            {
                var purchaseBasketResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(purchaseBasketResponse);
            }
            else
            {
                var purchaseBasketResponse = new ServerResponse<string>
                {
                    Value = "purchase success",
                };
                return Ok(purchaseBasketResponse);
            }
        }

        [HttpPost]
        [Route("Store/{storeId}/Products/Remove")]
        public async Task<ObjectResult> RemoveProduct([Required][FromQuery]int tokenId, [FromRoute] int storeId, [FromBody] ProductDto product)
        {
            if(product.Id is null) return BadRequest("product must contain id");
            Response response = await Task.Run(() => _marketService.RemoveProduct(tokenId, storeId, (int)product.Id));
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<string>.OkResponse("remove product success"));
            }
        }

        [HttpPost]
        [Route("Store/{storeId}/Products/Add")]
        public async Task<ActionResult<int>> AddProduct([Required][FromQuery]int tokenId, [FromRoute] int storeId, [FromBody] ProductDto product)
        {
            if(!product.IsValidCreate()) return BadRequest("product must contain store id and product name");
            Response<int> response = await Task.Run(() => _marketService.AddProduct(storeId, tokenId, product.ProductName, product.SellMethod, product.ProductDescription, (double)product.Price, product.Category, product.Quantity, product.AgeLimit));
            if (response.ErrorOccured)
            {
                var addProductResponse = new ServerResponse<string>
                {
                    ErrorMessage = response.ErrorMessage,
                };
                return BadRequest(addProductResponse);
            }
            else
            {
                var addProductResponse = new ServerResponse<int>
                {
                    Value = response.Value,
                };
                return Ok(addProductResponse);
            }
        }              
        
        [HttpPut]
        [Route("Store/{storeId}/Products/{productId}")]
        public async Task<ObjectResult> UpdateProduct([Required][FromQuery]int tokenId, [FromRoute] int storeId, [FromRoute] int productId, [FromBody] ProductDto product)
        {
            Response response = product.Price is not null ? await Task.Run(() => _marketService.UpdateProductPrice(tokenId, storeId, productId, (double)product.Price)) : await Task.Run(() => _marketService.UpdateProductQuantity(tokenId, storeId, productId, product.Quantity));
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<string>.OkResponse("update product success"));

            }
        }                        

        // [HttpPost]
        // [Route("search")]
        // public ActionResult<Response<List<ProductResultDto>>> Search([FromBody] SearchRequest request)
        // {
        //     Response<List<ProductResultDto>> response = _marketService.SearchByCategory(request.SessionId, request.Word, request.SearchType, request.FilterType, request.LowPrice, request.HighPrice, request.LowRate, request.HighRate, request.Category);
        //     if (response.ErrorOccured)
        //     {
        //         return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
        //     }
        //     else
        //     {
        //         return Ok(ServerResponse<List<SProduct>>.OkResponse(response.Value));
        //     }
        // }


        [HttpPost]
        [Route("Store/{storeId}/PurchuseHistory")]
        public ActionResult<Response<List<PurchaseResultDto>>> ShowShopPurchaseHistory([Required][FromQuery]int tokenId, [FromRoute] int storeId)
        {
            Response<List<PurchaseResultDto>> response = _marketService.GetPurchaseHistory(storeId, tokenId);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<List<PurchaseResultDto>>.OkResponse(response.Value));
            }
        }        
    }
}