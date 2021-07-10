using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AdeCartAPI.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]

    [Route("api/{username}/carts/{cartId}/orders/{orderId}/payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {

        readonly IOrder _order;
        readonly IOrderCart _cart;
        readonly AdeCartService cartService;

        public PaymentController(IOrder _order,AdeCartService cartService, IOrderCart _cart)
        {
            this._order = _order;
            this.cartService = cartService;
            this._cart = _cart;
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="cartId">
        ///the user's cart id
        ///</param>
        ///<param name="orderId">
        ///the user's order id
        ///</param>
        /// <summary>
        /// Pay for order
        /// </summary>
        /// 
        /// 
        /// <returns>A string status</returns>
        [HttpPost]
        public async Task<ActionResult> PayCharge(string username,int cartId,int orderId)
        {
            try
            {
                cartService.GetSecrets();

                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart == null) return NotFound();
                if (cart.OrderStatus == 1) return BadRequest("Order is been processed");

                var currentOrder = _order.GetOrder(orderId);
                if (currentOrder.OrderId == 0) return NotFound();

                var price = cartService.GetPrice(currentOrder);

                var charge = cartService.SetCharge(price, currentUser.Email);

                var pendingCharge = await cartService.InitializeCharge(charge);
                 var verification = JsonConvert.DeserializeObject<Verification>(pendingCharge);
                var pin = cartService.CreatePin(verification.Data.Reference);

                var content = await cartService.Submit_Pin(pin);

                var status = JsonConvert.DeserializeObject<Reciept>(content).Data.Status;
                if (status == "success")
                {
                    await cartService.UpdateOrder(cart);
                    await cartService.UpdateItem(currentOrder);
                    return Ok("Successful, Your order is been processed");
                }
                return BadRequest("Try Again");
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
           
            
        }
       

    }
}
