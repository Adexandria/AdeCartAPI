using System;
using AutoMapper;
using AdeCartAPI.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using AdeCartAPI.Model;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AdeCartAPI.UserModel;

namespace AdeCartAPI.Service
{
    public class AdeCartService
    {
        readonly UserManager<User> user;
        readonly ITemInterface _Item;
        readonly IMapper mapper;
        readonly IOrderCart _cart;
        private object key;
        private string endpoint;
        readonly IConfiguration _config;

        public AdeCartService(UserManager<User> user, ITemInterface _Item, IMapper mapper, IOrderCart _cart, IConfiguration _config)
        {
            this.user = user;
            this._Item = _Item;
            this.mapper = mapper;
            this._cart = _cart;
            this._config = _config;
        }

        public async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }

        public UserAddress InsertAddress(UserAddress address,string userId, int addressId) 
        {
            address.AddressId = addressId;
            address.UserId = userId;
            return address;
        }
        public async Task UpdateOrderCart(OrderCartData cart)
        {
            var orderCart = mapper.Map<OrderCart>(cart);
            orderCart.OrderStatus = OrderStatus.Processing;
            await _cart.UpdateCart(orderCart);
        }

        public async Task UpdateItem(List<Order> orders)
        {
            foreach(var order in orders) 
            {
                order.Item.AvailableItem = order.Item.AvailableItem - order.Quantity;
                if (order.Item.AvailableItem == 0)
                {
                    await _Item.DeleteItem(order.ItemId);
                }
                await _Item.UpdateItem(order.Item);
            }
        }

        public Charge SetCharge(int price, string email)
        {
            var charge = new Charge
            {
                Amount = (price).ToString(),
                Email = email,
                CardDetails = new Card()
            };
            return charge;
        }

        public async Task<string> Submit_Pin(Pin pin)
        {
            try
            {
                HttpClient client = GetClient();
                var url = endpoint + "/submit_pin";
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var json = JsonConvert.SerializeObject(pin);
                return await GetContent(httpResponse, json, url, client);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<string> InitializeCharge(Charge charge)
        {
            try
            {
                var client = GetClient();
                var url = endpoint;
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var json = JsonConvert.SerializeObject(charge);
                return await GetContent(httpResponse, json, url, client);
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        public async Task<string> GetContent(HttpResponseMessage httpResponse, string json, string url, HttpClient client)
        {
            using (StringContent content = new StringContent(json))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpResponse = await client.PostAsync(url, content);
            }
            string contentString = await httpResponse.Content.ReadAsStringAsync();
            var newContent = JToken.Parse(contentString).ToString();
            return newContent;
        }

        public Pin CreatePin(string reference)
        {
            Pin pin = new Pin
            {
                Reference = reference
            };
            return pin;
        }

        public int GetPrice(List<Order> currentOrders)
        {
            int price = 0;
            foreach(var currentOrder in currentOrders) 
            {
                price = currentOrder.Quantity * currentOrder.Item.ItemPrice;
            }
           return price;
        }

        public void GetSecrets()
        {
            key = _config["paystack_Secret"];
            endpoint = _config["paystack_Endpoint"];
        }

        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
            return client;
        }

        public int IsQuantity(int availableItem, int quantity)
        {
            if (quantity > availableItem)
            {
                quantity = availableItem;
                return quantity;
            }
            return quantity;
        }
        public List<Order> GetOrders(List<Order> currentOrder)
        {

            foreach (var order in currentOrder)
            {
                var item = _Item.GetItemById(order.ItemId);
                order.Item = item;
            }
            return currentOrder;
        }

        public Order GetOrder(Order currentOrder)
        {
            var item = _Item.GetItemById(currentOrder.ItemId);
            currentOrder.Item = item;
            return currentOrder;
        }

        public int IsAllow(OrderCartData cart)
        {
            if (cart.OrderStatus == 0)
                return cart.OrderCartId;
            return 0;
        }

        public Order UpdateOrder(int itemId, int cartId, int orderId, int quantity)
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

        public Item UpdateItem(ItemUpdate itemUpdate, Item item)
        {
            if (string.IsNullOrEmpty(itemUpdate.Name) || itemUpdate.Description != "string")
            {
                item.ItemName = itemUpdate.Name;
            }
            if (itemUpdate.Price != 0)
            {
                item.ItemPrice = itemUpdate.Price;
            }
            if (string.IsNullOrEmpty(itemUpdate.Description)|| itemUpdate.Description != "string")
            {
                item.ItemDescription = itemUpdate.Description;
            }
            if (itemUpdate.AvailableItem != 0)
            {
                item.AvailableItem = itemUpdate.AvailableItem;
            }
            return item;
        }
    }
}
