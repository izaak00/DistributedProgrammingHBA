using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Humanizer.On;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.Controllers
{
    public class ProductCatalogueController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7074";

        public ProductCatalogueController()
        {
            _httpClient = new HttpClient();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetProductDetails()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetProductDetails(string productName)
        {
            var requestUrl = $"{ApiUrl}/GetProduct/{productName}";

            // Make a GET request to the API to retrieve user details
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                List<Product> productDetails = await response.Content.ReadFromJsonAsync<List<Product>>();
                return View(productDetails);
            }

            return View("Error");
        }
    }
}
