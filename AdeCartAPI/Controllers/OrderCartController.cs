using AdeCartAPI.DTO;
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
    [Route("api/{username}/carts")]
    [ApiController]
    // [Authorize]
    public class OrderCartController : ControllerBase
    {
        readonly IOrderCart _cart;
        readonly IMapper mapper;
        readonly UserManager<User> user;
        public OrderCartController(IOrderCart _cart, IMapper mapper, UserManager<User> user)
        {
            this._cart = _cart;
            this.mapper = mapper;
            this.user = user;
        }
        [HttpGet]
        public async Task<ActionResult> GetCarts(string username)
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var carts = _cart.GetCarts(currentUser.Id);
            return Ok(carts);
        }
        [HttpGet("{id}", Name ="OrderCart")]
        public async Task<ActionResult> GetCart(OrderCart orderCart,string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var cart = _cart.GetCart(orderCart);
            return Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult> CreateCart(string username,OrderCart cart) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var carts = _cart.GetCarts(currentUser.Id);
            if(carts.Select(s => s.OrderStatus == 0).Contains(true)) 
            {
                return BadRequest("Cart already exist");
            }
            cart.UserId = currentUser.Id;
            await _cart.AddCart(cart);
            return Ok("Created");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCart(string username,OrderCart cart) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            cart.UserId = currentUser.Id;
            var currentCart = _cart.GetCart(cart);
            if (currentCart == null) return NotFound();
            await _cart.UpdateCart(cart);
            return Ok("Successful");
        }
        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
    }
}
