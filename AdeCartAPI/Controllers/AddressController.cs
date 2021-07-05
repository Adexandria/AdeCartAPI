using AdeCartAPI.Model;
using AdeCartAPI.UserModel;
using AdeCartAPI.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [Route("api/user/{username}/address")]
    [ApiController]
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
        public async Task<ActionResult> Post(string username,AddressCreate userAddress)
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var newAddress = mapper.Map<UserAddress>(userAddress);
            var userId = mapper.Map<UserAddress>(currentUser);
            newAddress.UserId = userId.UserId;
            await address.CreateAddress(newAddress);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult>Put(string username,AddressUpdate userAddress) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            var newAddress = mapper.Map<UserAddress>(userAddress);
            var userId = mapper.Map<UserAddress>(currentUser);
            newAddress.UserId = userId.UserId;
            await address.UpdateAddress(newAddress);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(string username,Guid id) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();
            await address.DeleteAddress(id);
            return Ok();
        }
        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
    }
}
