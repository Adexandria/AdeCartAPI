using System;
using System.Net;
using Newtonsoft.Json;
using AdeCartAPI.Model;
using AdeCartAPI.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;


namespace AdeCartAPI.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]

    [Route("api/{username}/carts/{cartId}/orders/payment")]
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
        /// <summary>
        /// Pay for order
        /// </summary>
        /// 
        /// 
        /// <returns>A string status</returns>
        [HttpPost]
        public async Task<ActionResult> PayCharge(string username,int cartId)
        {
            try
            {
                cartService.GetSecrets();

                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("Username not found");

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("cart not found");
                if (cart.OrderStatus == 1) return BadRequest("Order is been processed");

                var orders = _order.GetOrders(cartId);
                var price = cartService.GetPrice(orders);

                var charge = cartService.SetCharge(price, currentUser.Email);

                var pendingCharge = await cartService.InitializeCharge(charge);
                 var verification = JsonConvert.DeserializeObject<Verification>(pendingCharge);
                var pin = cartService.CreatePin(verification.Data.Reference);

                var content = await cartService.Submit_Pin(pin);

                var status = JsonConvert.DeserializeObject<Reciept>(content).Data.Status;
                if (status == "success")
                {
                    await cartService.UpdateOrderCart(cart);
                    await cartService.UpdateItem(orders);
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
