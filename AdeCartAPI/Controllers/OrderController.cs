using AdeCartAPI.Model;
using AdeCartAPI.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [Route("api/{username}/cart/{cartId}/orders")]         
    [ApiController]
    //[Authorize]
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
        [HttpGet]
        public async Task<ActionResult> GetOrders(string username,int cartId) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var carts = _cart.GetCarts(currentUser.Id);
            if(carts == null)
            {
                await _cart.AddCart(currentUser.Id);
            }
            var currentOrders = _order.GetOrders(cartId);
            return Ok(currentOrders);
        }
        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrder(string username, int cartId,int orderId) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var cart = _cart.GetCart(cartId,currentUser.Id);
            if (cart == null)
            {
                await _cart.AddCart(currentUser.Id);
            }
            var currentOrder = _order.GetOrder(orderId);
            return Ok(currentOrder);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrder(string username, int cartId,Order order) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null)
            {
                await _cart.AddCart(currentUser.Id);
            }
            order.OrderCartId = cartId;
            await _order.AddOrder(order);
            return Ok("Created");
        }
        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder(string username, int cartId,[FromBody] int itemId,int orderId) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null)
            {
                await _cart.AddCart(currentUser.Id);
            }
            var currentOrder = _order.GetOrder(orderId);
            if(currentOrder == null) 
            {
                return NotFound();
            }
            var item = _Item.GetItemById(itemId);
            if (item == null) return NotFound("Item doesn't exist");
            var updateOrder = new Order
            {
                ItemId = itemId,
                OrderCartId = cartId,
                OrderId = orderId
            };
           
            await _order.UpdateOrder(updateOrder);
            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(string username, int cartId, int orderId) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null)
            {
                await _cart.AddCart(currentUser.Id);
            }
            var currentOrder = _order.GetOrder(orderId);
            if (currentOrder == null)
            {
                return NotFound();
            }
            await _order.DeleteOrder(orderId);
            return Ok();
        }
        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
    }
}
