using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Controllers;
using static Humanizer.On;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7271";

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
    }
}
