using AdeCartAPI.Model;
using AdeCartAPI.Service;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AdeCartAPI.Controllers
{
    [Route("api/{username}/carts/{cartId}/orders/{orderId}/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        string key = null;
        string endpoint = null;

        Verification verification;

        private readonly IConfiguration _config;
        readonly IOrder _order;
        readonly IOrderCart _cart;
        readonly IMapper mapper;
        readonly ITemInterface _Item;
        readonly UserManager<User> user;

        public PaymentController(IConfiguration _config, IOrder _order, IMapper mapper, UserManager<User> user, IOrderCart _cart, ITemInterface _Item)
        {
            this._config = _config;
            this._order = _order;
            this.mapper = mapper;
            this.user = user;
            this._cart = _cart;
            this._Item = _Item;
        }
        [HttpPost]
        public async Task<ActionResult> PayCharge(string username,int cartId,int orderId) 
        {  
            GetSecrets();
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound();

            var cart = _cart.GetCart(cartId, currentUser.Id);
            if (cart == null) await _cart.AddCart(currentUser.Id);

            var currentOrder = _order.GetOrder(orderId);
            if (currentOrder.OrderId == 0) return NotFound();
            var price = GetPrice(currentOrder);
            var charge = SetCharge(price, currentUser.Email);
            var pendingCharge = await InitializeCharge(charge);
            verification = JsonConvert.DeserializeObject<Verification>(pendingCharge);
            var pin = CreatePin(verification.Data.Reference);
            var reciept = await Submit_Pin(pin); 
            return Ok(reciept);
        }

       
        private Charge SetCharge(int price,string email) 
        {
            var charge = new Charge
            {
                Amount = (price * 10000).ToString(),
                Email = email,
                CardDetails = new Card()
            };
            return charge;
        }

        private async Task<string> Submit_Pin(Pin pin)
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

        private async Task<string> Submit(OTP otp)
        {
            try
            {
                HttpClient client = GetClient();
                var url = endpoint + "/submit_otp";
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var json = JsonConvert.SerializeObject(otp);
                return await GetContent(httpResponse, json, url, client);
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        private async Task<string> InitializeCharge(Charge charge)
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

        private async Task<string> GetContent(HttpResponseMessage httpResponse, string json, string url, HttpClient client)
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
        private Pin CreatePin(string reference)
        {
            Pin pin = new Pin
            {
                Reference = reference
            };
            return pin;
        }
        private OTP CreateOTP(string reference) 
        {
            OTP otp = new OTP
            {
                Reference = reference
            };
            return otp;
        }
        private int GetPrice(Order currentOrder) 
        {
            currentOrder = GetOrder(currentOrder);
            int price = currentOrder.Quantity * currentOrder.Item.AvailableItem;
            return price;
        }

        private void GetSecrets() 
        {
            key = _config["paystack_Secret"];
            endpoint = _config["paystack_Endpoint"];
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization",$"Bearer {key}");
            return client;
        }

        private Order GetOrder(Order currentOrder)
        {
            var item = _Item.GetItemById(currentOrder.ItemId);
            currentOrder.Item = item;
            return currentOrder;
        }

        private async Task<User> GetUser(string userName)
        {
            return await user.FindByNameAsync(userName);
        }
    }
}
