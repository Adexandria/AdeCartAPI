using System;
using AutoMapper;
using System.Net;
using AdeCartAPI.Service;
using AdeCartAPI.UserModel;
using AdeCartAPI.UserService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;


namespace AdeCartAPI.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Returns no content")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]

    [Route("api/user/{username}/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        readonly IAddress address;
        readonly IMapper mapper;
        readonly AdeCartService cartService;
        public AddressController(IAddress address,IMapper mapper, AdeCartService cartService)
        {
            this.address = address;
            this.mapper = mapper;
            this.cartService = cartService;
        }

        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="userAddress">
        ///the user's Address
        ///</param>
        /// <summary>
        /// Add the user's Address
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost]
        public async Task<ActionResult> Post(string username, AddressCreate userAddress)
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var newAddress = mapper.Map<UserAddress>(userAddress);
                var userId = mapper.Map<UserAddress>(currentUser);
                newAddress.UserId = userId.UserId;
                await address.CreateAddress(newAddress);
                return Ok("Address successfully added");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }

        ///<param name="username">
        ///the user's username
        ///</param>
        ///<param name="userAddress">
        ///the user's Address
        ///</param>
        /// <summary>
        /// update the user's Address
        /// </summary>
        /// 
        /// <returns>None</returns>
        [HttpPut]
        public async Task<ActionResult> Put(string username, AddressUpdate userAddress)
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var updateAddress = mapper.Map<UserAddress>(userAddress);
                var isExist = address.GetAddress(updateAddress.AddressId);
                if (isExist == 0) return NotFound("The address doesn't exist");
                var userId = mapper.Map<UserAddress>(currentUser);
                updateAddress.UserId = userId.UserId;
                await address.UpdateAddress(updateAddress);
                return Ok();
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
        ///the address id
        ///</param>
        /// <summary>
        /// Delete the user's Address
        /// </summary>
        /// 
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string username,int id) 
        {
            try
            {
                var currentUser = await cartService.GetUser(username);
                if (currentUser == null) return NotFound();
                var isExist = address.GetAddress(id);
                if (isExist == 0) return NotFound("The address doesn't exist");
                await address.DeleteAddress(id);
                return NoContent();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
    }
}
