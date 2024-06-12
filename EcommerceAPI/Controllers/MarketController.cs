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
        // public async Task<ObjectResult> Appoint([Required][FromQuery]string identifier, [FromBody] StaffMemberDto staffMember)
        // {
        //     var role = GetRoleByName(staffMember.Role);
        //     Response<int> response = await Task.Run(() => _marketService.AddStaffMember(staffMember.StoreId, identifier, new Role(new(role), new(), staffMember.StoreId, staffMember.Id), staffMember.Id));
            
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
        // public async Task<ObjectResult> RemoveAppoint([Required][FromQuery]string identifier, [FromBody] StaffMemberDto staffMember)
        // {
        //     Response response = await Task.Run(() => _marketService.RemoveStaffMember(staffMember.StoreId, identifier, staffMember.Id));
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
        public async Task<ObjectResult> CloseStore([Required][FromQuery]string identifier, [FromRoute] int storeId)
        {
            Response response = await Task.Run(() => _marketService.CloseStore(identifier, storeId));
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
        public async Task<ObjectResult> OpenStore([Required][FromQuery]string identifier, [FromRoute] int storeId)
        {
            Response response = await Task.Run(() => _marketService.OpenStore(identifier, storeId));
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
        public async Task<ObjectResult> PurchaseCart([Required][FromQuery]string identifier, [FromBody] PurchaseDto purchaseInfo)
        {
            if(!purchaseInfo.IsValid()) return BadRequest("all fileds are required");
            var shippingDetails = purchaseInfo.ShippingInfo();
            var paymentDetails = purchaseInfo.PaymentInfo();
            Response response = await Task.Run(() => _marketService.PurchaseCart(identifier, paymentDetails, shippingDetails));
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
        public async Task<ObjectResult> RemoveProduct([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] ProductDto product)
        {
            if(product.Id is null) return BadRequest("product must contain id");
            Response response = await Task.Run(() => _marketService.RemoveProduct(storeId, identifier, (int)product.Id));
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
        public async Task<ActionResult<int>> AddProduct([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] ProductDto product)
        {
            if(!product.IsValidCreate()) return BadRequest("product must contain store id and product name");
            Response<int> response = await Task.Run(() => _marketService.AddProduct(storeId, identifier, product.ProductName, product.SellMethod, product.ProductDescription, (double)product.Price, product.Category, product.Quantity, product.AgeLimit));
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
        public async Task<ObjectResult> UpdateProduct([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromRoute] int productId, [FromBody] ProductDto product)
        {
            List<Task<Response>> tasks =
            [
                Task.Run(() => _marketService.UpdateProductPrice(storeId, identifier, productId, (double)product.Price)),
                Task.Run(() => _marketService.UpdateProductQuantity(storeId, identifier, productId, product.Quantity)),
            ];
            var responses = await Task.WhenAll(tasks.ToArray());

            if (responses.Any(response => response.ErrorOccured))
            {
                foreach(var response in responses){
                    if(response.ErrorOccured)
                        return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
                }
            }
            return Ok(ServerResponse<string>.OkResponse("update product success"));
        }                        

        [HttpGet]
        [Route("Search/KeyWords")]
        public ActionResult<Response<List<ProductResultDto>>> SearchByKeywords([Required][FromQuery]string identifier, [FromQuery] List<string> keyword)
        {
            var products = new List<ProductResultDto>();
            foreach(var word in keyword){
                Response<List<ProductResultDto>> response = _marketService.SearchByKeywords(word);
                if (response.ErrorOccured)
                {
                    return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
                }else{
                    products.AddRange(response.Value);
                }
            }

            return Ok(ServerResponse<List<ProductResultDto>>.OkResponse(products));
        }

        [HttpGet]
        [Route("Search/Name")]
        public ActionResult<Response<List<ProductResultDto>>> SearchByNames([Required][FromQuery]string identifier, [FromQuery] List<string> name)
        {
            var products = new List<ProductResultDto>();
            foreach(var word in name){
                Response<List<ProductResultDto>> response = _marketService.SearchByName(word);
                if (response.ErrorOccured)
                {
                    return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
                }else{
                    products.AddRange(response.Value);
                }
            }

            return Ok(ServerResponse<List<ProductResultDto>>.OkResponse(products));
        }

        [HttpGet]
        [Route("Search/Category")]
        public ActionResult<Response<List<ProductResultDto>>> SearchByCategory([Required][FromQuery]string identifier, [FromQuery] List<string> category)
        {
            var products = new List<ProductResultDto>();
            foreach(var word in category){
                Response<List<ProductResultDto>> response = _marketService.SearchByCategory(word);
                if (response.ErrorOccured)
                {
                    return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
                }else{
                    products.AddRange(response.Value);
                }
            }

            return Ok(ServerResponse<List<ProductResultDto>>.OkResponse(products));
        }

        [HttpGet]
        [Route("Store/Name")]
        public ActionResult<Response<string>> GetStoreById([Required][FromQuery]string identifier, [FromQuery] int storeId)
        {            
            Response<string> response = _marketService.GetStoreById(storeId);
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
                var addProductResponse = new ServerResponse<string>
                {
                    Value = response.Value,
                };
                return Ok(addProductResponse);
            }
        }


        [HttpPost]
        [Route("Store/{storeId}/PurchuseHistory")]
        public ActionResult<Response<List<PurchaseResultDto>>> ShowShopPurchaseHistory([Required][FromQuery]string identifier, [FromRoute] int storeId)
        {
            Response<List<PurchaseResultDto>> response = _marketService.GetPurchaseHistoryByStore(storeId, identifier);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<List<PurchaseResultDto>>.OkResponse(response.Value));
            }
        }        

        [HttpGet]
        [Route("Store/{storeId}/GetRules")]
        public ActionResult<Response<List<RuleResultDto>>> GetStoreRules([Required][FromQuery]string identifier, [FromRoute] int storeId)
        {
            Response<List<RuleResultDto>> response = _marketService.GetStoreRules(storeId, identifier);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<List<RuleResultDto>>.OkResponse(response.Value));
            }
        }        

        [HttpPost]
        [Route("Store/{storeId}/AddRule")]
        public ActionResult<Response<int>> CreateStoreRule([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] RuleDto rule)
        {
            if(!rule.IsValid()) return BadRequest("rule must have a subject");
            Response<int> response = _marketService.AddSimpleRule(identifier, storeId, rule.Subject);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<int>.OkResponse(response.Value));
            }
        }   
        
        [HttpPost]
        [Route("Store/{storeId}/AddRule/Quantity")]
        public ActionResult<Response<int>> CreateStoreQuantityRule([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] QuantityRuleDto rule)
        {
            if(!rule.IsValid()) return BadRequest("rule must have a subject");
            Response<int> response = _marketService.AddQuantityRule(identifier, storeId, rule.Subject, rule.MinQuantity, rule.MaxQuantity);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<int>.OkResponse(response.Value));
            }
        }         

        [HttpPost]
        [Route("Store/{storeId}/AddRule/TotalPrice")]
        public ActionResult<Response<int>> CreateStoreTotalPriceRule([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] TotalPriceRuleDto rule)
        {
            if(!rule.IsValid()) return BadRequest("rule must have a subject");
            Response<int> response = _marketService.AddTotalPriceRule(identifier, storeId, rule.Subject, rule.TargetPrice);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<int>.OkResponse(response.Value));
            }
        } 

        [HttpPost]
        [Route("Store/{storeId}/AddRule/CompositeRule")]
        public ActionResult<Response<int>> CreateStoreCompositeRule([Required][FromQuery]string identifier, [FromRoute] int storeId, [FromBody] CompositeRuleDto rule)
        {
            if(!rule.IsValid()) return BadRequest("composite rule must have rules");
            Response<int> response = _marketService.AddCompositeRule(identifier, storeId, rule.Operator, rule.Rules);
            if (response.ErrorOccured)
            {
                return BadRequest(ServerResponse<string>.BadResponse(response.ErrorMessage));
            }
            else
            {
                return Ok(ServerResponse<int>.OkResponse(response.Value));
            }
        }   
    }
}
