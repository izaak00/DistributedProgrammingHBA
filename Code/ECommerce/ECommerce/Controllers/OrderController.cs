﻿using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Controllers;
using static Humanizer.On;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using Order = ECommerce.Models.Order;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://orderapiimage-j7lgba4epq-uc.a.run.app";

        public OrderController(ILogger<UserController> logger)
        {
            _httpClient = new HttpClient();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> OrderDetails(string productName, string productPrice)
        {
            Order order = new Order()
            {
                ProductName = productName,
                Price = productPrice,
                OrderId = Guid.NewGuid().ToString(),
                Owner = User.Identity.Name
            };

            // Serialize the Order object to JSON
            string orderJson = JsonConvert.SerializeObject(order);

            // Store the object in TempData
            TempData["MyObject"] = orderJson;

            if (order != null)
            {
                return RedirectToAction("PaymentProcedure", "Payment");
            }

            return View("Error");  
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            string email = User.Identity.Name;
            var requestUrl = $"{ApiUrl}/orderpaymentshipping/{email}";

            // Make a GET request to the API to retrieve user details
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                List<OrderPaymentShipping> orderPaymentShippingList = await response.Content.ReadFromJsonAsync<List<OrderPaymentShipping>>();
                return View(orderPaymentShippingList);
            }

            return View("Error");
        }
    }
}
