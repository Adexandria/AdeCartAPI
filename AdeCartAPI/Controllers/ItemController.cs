using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AdeCartAPI.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        readonly ITemInterface _Item;
        readonly IMapper mapper;
        public ItemController(ITemInterface _Item, IMapper mapper)
        {
            this._Item = _Item;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<ItemDTO>> GetItems()
        {
            var items = _Item.GetItems;
            var currentItems = mapper.Map<List<ItemDTO>>(items);
            return Ok(currentItems);
        }

        [AllowAnonymous]
        [HttpGet("{itemName}", Name = "GetItem")]
        public ActionResult<ItemDTO> GetItem(string itemName)
        {
            var item = _Item.GetItem(itemName);
            var currentItem = mapper.Map<ItemDTO>(item);
            return Ok(currentItem);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> AddItem(ItemCreate itemCreate)
        {
            var newItem = mapper.Map<Item>(itemCreate);
            await _Item.AddItem(newItem);
            var item = _Item.GetItem(newItem.ItemName);
            var currentItem = mapper.Map<ItemDTO>(item);
            return CreatedAtRoute("GetItem", new { itemName = currentItem.Name}, currentItem);

        }
       // [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateItem(ItemUpdate itemUpdate)
        {
            var item = _Item.GetItem(itemUpdate.Name);
            if (item == null) return NotFound("Item doesn't exist");
            var updatedItem = mapper.Map<Item>(itemUpdate);
            await _Item.UpdateItem(updatedItem);
            return Ok("Successful");
        }

       // [Authorize(Roles = "Admin")]
        [HttpDelete("{itemName}")]
        public async Task<ActionResult> DeleteItem(string itemName) 
        {
            var item = _Item.GetItem(itemName);
            if (item == null) return NotFound("Item doesn't exist");
            await _Item.DeleteItem(item.ItemId);
            return NoContent();
        } 
            
    }
}
