using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            Order o = new Order()
            {
                ProductName = productName,
                Price = productPrice,
                OrderId = Guid.NewGuid().ToString(),
                Owner = User.Identity.Name
            };
        
            var requestUrl = $"{ApiUrl}/order";

            // Serialize the Login object to JSON
            var requestData = JsonConvert.SerializeObject(o);

            // Create the HTTP request content with JSON data
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            // Make a GET request to the API to retrieve user details
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

            // here you should redirect user to the payment page 

            //if (response.IsSuccessStatusCode)
            //{
            //    List<Product> productDetails = await response.Content.ReadFromJsonAsync<List<Product>>();
            //    return View(productDetails);
            //}

            return View("Error");
        }
    }
}
