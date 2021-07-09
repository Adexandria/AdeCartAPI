using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AdeCartAPI.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers

{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Returns no content")]

    [Route("api/{username}/carts/{cartId}/orders")]         
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        readonly IOrder _order;
        readonly IOrderCart _cart;
        readonly IMapper mapper;
        readonly ITemInterface _Item;
        readonly UserManager<User> user;

        public OrderController(IOrder _order, IMapper mapper, UserManager<User> user, IOrderCart _cart, ITemInterface _Item)
        {
            this._order = _order;
            this.mapper = mapper;
            this.user = user;
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
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var carts = _cart.GetCarts(currentUser.Id);
            if(carts == null) await _cart.AddCart(currentUser.Id);

            var currentOrders = _order.GetOrders(cartId);
            currentOrders = GetOrders(currentOrders);

            var mappedOrders = mapper.Map<List<OrderDTO>>(currentOrders);
            return Ok(mappedOrders);
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
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var cart = _cart.GetCart(cartId,currentUser.Id);
            if (cart == null) await _cart.AddCart(currentUser.Id);

            var currentOrder = _order.GetOrder(orderId);
            if (currentOrder.OrderId == 0) return NotFound();

            currentOrder = GetOrder(currentOrder);

            var mappedOrder = mapper.Map<OrderDTO>(currentOrder);
            return Ok(mappedOrder);
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
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null) await _cart.AddCart(currentUser.Id);

            cartId = IsAllow(cart);
            if(cartId == 0) return BadRequest("Wrong Cart!!!");

            var item = _Item.GetItemById(newOrder.ItemId);
            if (item == null) return NotFound("Item doesn't exist");

            var quantity = IsQuantity(item.AvailableItem, newOrder.Quantity);

            var mappedOrder = mapper.Map<Order>(newOrder);

            mappedOrder.OrderCartId = cartId;
            mappedOrder.Quantity = quantity;

            await _order.AddOrder(mappedOrder);
            return Ok("Created");
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
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null) await _cart.AddCart(currentUser.Id);

            var currentOrder = _order.GetOrder(orderId);
            if(currentOrder == null) return NotFound();

            var item = _Item.GetItemById(updateOrder.ItemId);
            if (item == null) return NotFound("Item doesn't exist");

            var updatedOrder = UpdateOrder(updateOrder.ItemId, cartId, orderId,updateOrder.Quantity);
            await _order.UpdateOrder(updatedOrder);
            return NoContent();
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
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null)await _cart.AddCart(currentUser.Id);

            var currentOrder = _order.GetOrder(orderId);
            if (currentOrder == null)return NotFound();

            await _order.DeleteOrder(orderId);
            return Ok();
        }
        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
        private Order UpdateOrder(int itemId,int cartId,int orderId,int quantity) 
        {
            var updateOrder = new Order
            {
                ItemId = itemId,
                OrderCartId = cartId,
                OrderId = orderId,
                Quantity = quantity
            };
            return updateOrder;
        }
        private int IsAllow(OrderCartData cart) 
        {
            if(cart.OrderStatus == 0) 
            return cart.OrderCartId;
            return 0;
        }
        private int IsQuantity(int availableItem,int quantity)
        {
            if(quantity > availableItem) 
            {
                quantity = availableItem;
                return quantity;
            }
            return quantity;
        }
        private List<Order> GetOrders(List<Order> currentOrder) 
        {
            var items = _Item.GetItems;
            foreach(var item in items)
            {
               foreach(var order in currentOrder) 
               {
                    order.Item = item;
               }
            }
            return currentOrder;
        } 
        private Order GetOrder(Order currentOrder) 
        {
            var item = _Item.GetItemById(currentOrder.ItemId);
            currentOrder.Item = item;
            return currentOrder;
        }
    }
}
