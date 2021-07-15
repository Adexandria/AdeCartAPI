using System;
using AutoMapper;
using System.Net;
using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AdeCartAPI.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;


namespace AdeCartAPI.Controllers

{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Returns no content")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]

    [Route("api/{username}/carts/{cartId}/orders")]         
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        readonly IOrder _order;
        readonly IOrderCart _cart;
        readonly IMapper mapper;
        readonly ITemInterface _Item;
        readonly AdeCartService cartService;

        public OrderController(IOrder _order, IMapper mapper, AdeCartService cartService, IOrderCart _cart, ITemInterface _Item)
        {
            this._order = _order;
            this.mapper = mapper;
            this.cartService = cartService;
            this._cart = _cart;
            this._Item = _Item;
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="cartId">
        ///the user's cart id
        ///</param>
        /// <summary>
        /// get all orders
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet]
        public async Task<ActionResult> GetOrders(string username,int cartId)
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("User not found");

                var cart = _cart.GetCart(cartId,currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("Cart doesn't exist");

                var currentOrders = _order.GetOrders(cartId);
                currentOrders = cartService.GetOrders(currentOrders);

                var mappedOrders = mapper.Map<List<OrderDTO>>(currentOrders);
                return Ok(mappedOrders);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
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
        /// to get an individual order
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrder(string username, int cartId,int orderId) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("User not found");

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("Cart doesn't exist");

                var currentOrder = _order.GetOrder(orderId);
                if (currentOrder.OrderId == 0) return NotFound();

                currentOrder = cartService.GetOrder(currentOrder);

                var mappedOrder = mapper.Map<OrderDTO>(currentOrder);
                return Ok(mappedOrder);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="cartId">
        ///the user's cart id
        ///</param>
        ///<param name="newOrder">
        ///an object to create new order
        ///</param>
        /// <summary>
        /// create a new order
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost]
        public async Task<ActionResult> AddOrder(string username, int cartId,OrderCreate newOrder) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("User not found");

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("Cart doesn't exist");

                cartId = cartService.IsAllow(cart);
                if (cartId == 0) return BadRequest("Wrong Cart!!!");

                var item = _Item.GetItemById(newOrder.ItemId);
                if (item == null) return NotFound("Item doesn't exist");

                var quantity = cartService.IsQuantity(item.AvailableItem, newOrder.Quantity);

                var mappedOrder = mapper.Map<Order>(newOrder);

                mappedOrder.OrderCartId = cartId;
                mappedOrder.Quantity = quantity;

                await _order.AddOrder(mappedOrder);
                return Ok("Created Successfully");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="cartId">
        ///the user's cart id
        ///</param>
        ///<param name="updateOrder">
        ///an object used to update an order
        ///</param>
        ///<param name="orderId">
        ///the user's order id
        ///</param>
        /// <summary>
        /// Updates an order
        /// </summary>
        /// 
        /// 
        /// <returns>A string status</returns>
        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder(string username, int cartId,int orderId,OrderUpdate updateOrder)
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("User not found");

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("Cart doesn't exist");

                var currentOrder = _order.GetOrder(orderId);
                if (currentOrder.OrderId == 0) return NotFound();

                var item = _Item.GetItemById(updateOrder.ItemId);
                if (item == null) return NotFound("Item doesn't exist");

                var updatedOrder = cartService.UpdateOrder(updateOrder.ItemId, cartId, orderId, updateOrder.Quantity);
                await _order.UpdateOrder(updatedOrder);
                return Ok("Updated Successfully");

            }
            catch (Exception e )
            {

                return BadRequest(e.Message);
            }
           
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
        /// Delete an order
        /// </summary>
        /// 
        /// 
        /// <returns>A string status</returns>
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(string username, int cartId, int orderId) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound("User not found");

                var cart = _cart.GetCart(cartId, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound("Cart doesn't exist");

                var currentOrder = _order.GetOrder(orderId);
                if (currentOrder.OrderId == 0) return NotFound();

                await _order.DeleteOrder(orderId);
                return Ok("Successful");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
       
        
        
        
    }
}
