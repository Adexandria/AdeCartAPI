using System;
using System.Net;
using AutoMapper;
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

    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        readonly ITemInterface _Item;
        readonly IMapper mapper;
        readonly AdeCartService cartService;
        public ItemController(ITemInterface _Item, IMapper mapper, AdeCartService cartService)
        {
            this._Item = _Item;
            this.mapper = mapper;
            this.cartService = cartService;
        }
       
        /// <summary>
        /// Get all items
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<ItemDTO>> GetItems()
        {
            try
            {
                var items = _Item.GetItems;
                var currentItems = mapper.Map<List<ItemDTO>>(items);
                return Ok(currentItems);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }

        ///<param name="itemName">
        ///the item name
        ///</param>
        /// <summary>
        ///Get an individual item
        /// </summary>
        /// 
        /// <returns>A string status</returns>

        [AllowAnonymous]
        [HttpGet("{itemName}", Name = "GetItem")]
        public ActionResult<ItemDTO> GetItem(string itemName)
        {
            try
            {
                var item = _Item.GetItem(itemName);
                var currentItem = mapper.Map<ItemDTO>(item);
                return Ok(currentItem);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        ///<param name="itemCreate">
        ///an object used to create item
        ///</param>
        /// <summary>
        /// create a new item
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> AddItem(ItemCreate itemCreate)
        {
            try
            {
                var newItem = mapper.Map<Item>(itemCreate);
                await _Item.AddItem(newItem);
                var item = _Item.GetItem(newItem.ItemName);
                var currentItem = mapper.Map<ItemDTO>(item);
                return CreatedAtRoute("GetItem", new { itemName = currentItem.Name }, currentItem);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           

        }
        ///<param name="itemUpdate">
        ///an object used to update item
        ///</param>
        /// <summary>
        /// update an existing Item
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateItem(ItemUpdate itemUpdate)
        {
            try
            {
                var item = _Item.GetItemById(itemUpdate.Id);
                if (item == null) return NotFound("Item doesn't exist");
                var currentItem = cartService.UpdateItem(itemUpdate, item);
                await _Item.UpdateItem(currentItem);
                return Ok("Successful");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        ///<param name="itemName">
        ///an item Name
        ///</param>
        /// <summary>
        /// delete an existing Item
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{itemName}")]
        public async Task<ActionResult> DeleteItem(string itemName) 
        {
            try
            {
                var item = _Item.GetItem(itemName);
                if (item == null) return NotFound("Item doesn't exist");
                await _Item.DeleteItem(item.ItemId);
                return NoContent();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        } 

      
    }
}
