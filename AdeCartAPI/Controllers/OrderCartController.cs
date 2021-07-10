using System;
using System.Net;
using AutoMapper;
using System.Linq;
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

    [Route("api/{username}/carts")]
    [ApiController]
    [Authorize]
    public class OrderCartController : ControllerBase
    {
        readonly IOrderCart _cart;
        readonly IMapper mapper;
        readonly AdeCartService cartService;
        public OrderCartController(IOrderCart _cart, IMapper mapper, AdeCartService cartService)
        {
            this._cart = _cart;
            this.mapper = mapper;
            this.cartService = cartService;
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        /// <summary>
        /// gets User Cart
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet]
        public async Task<ActionResult> GetCarts(string username)
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var carts = _cart.GetCarts(currentUser.Id);
                var mappedCarts = mapper.Map<List<OrderCart>>(carts);
                var mappedCartsDTO = mapper.Map<List<OrderCartsDTO>>(mappedCarts);
                return Ok(mappedCartsDTO);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="id">
        ///the user's cart id
        ///</param>
        /// <summary>
        /// Get an individual cart
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet("{id}", Name ="OrderCart")]
        public async Task<ActionResult> GetCart(int id,string username) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var cart = _cart.GetCart(id, currentUser.Id);
                if (cart.OrderCartId == 0) return NotFound();
                var mappedCart = mapper.Map<OrderCart>(cart);
                var mappedCartDTO = mapper.Map<OrderCartDTO>(mappedCart);
                return Ok(mappedCartDTO);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        ///<param name="username">
        ///the user's username
        ///</param>
        /// <summary>
        ///Create a new cart
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost]
        public async Task<ActionResult> CreateCart(string username) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var carts = _cart.GetCarts(currentUser.Id);
                if (carts.Select(s => s.OrderStatus == 0).Contains(true))
                {
                    return BadRequest("Cart already exist");
                }
                await _cart.AddCart(currentUser.Id);
                return Ok("Created");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="updatecart">
        ///an object used to update a cart
        ///</param>
        /// <summary>
        /// To update cart
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPut()]
        public async Task<ActionResult> UpdateCart(string username,CartUpdate updatecart) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var cart = mapper.Map<OrderCart>(updatecart);
                var currentCart = _cart.GetCart(updatecart.OrderCartId, currentUser.Id);
                if (currentCart == null) return NotFound();
                await _cart.UpdateCart(cart);
                return Ok("Successful");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
    }
}
