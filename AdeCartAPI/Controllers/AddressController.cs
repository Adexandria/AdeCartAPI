using AdeCartAPI.Model;
using AdeCartAPI.UserModel;
using AdeCartAPI.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [Route("api/user/{username}/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        readonly IAddress address;
        readonly IMapper mapper;
        readonly UserManager<User> user;
        public AddressController(IAddress address, UserManager<User> user, IMapper mapper)
        {
            this.address = address;
            this.user = user;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> Post(string username, AddressCreate userAddress)
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var newAddress = mapper.Map<UserAddress>(userAddress);
            var userId = mapper.Map<UserAddress>(currentUser);
            newAddress.UserId = userId.UserId;
            await address.CreateAddress(newAddress);
            return Ok("Address successfully added");
        }
        [HttpPut]
        public async Task<ActionResult> Put(string username, AddressUpdate userAddress)
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var updateAddress = mapper.Map<UserAddress>(userAddress);
            var isExist = address.GetAddress(updateAddress.AddressId);
            if (isExist == 0) return NotFound("The address doesn't exist");
            var userId = mapper.Map<UserAddress>(currentUser);
            updateAddress.UserId = userId.UserId;
            await address.UpdateAddress(updateAddress);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string username,int id) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var isExist = address.GetAddress(id);
            if (isExist == 0) return NotFound("The address doesn't exist");
            await address.DeleteAddress(id);
            return NoContent();
        }
        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
    }
}
