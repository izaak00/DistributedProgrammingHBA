using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Models;
using Newtonsoft.Json;
using static Humanizer.On;
using System.Net.Http;
using System.Text;

namespace ECommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7114";

        public string OrderId { get; set; }

        public PaymentController()
        {
            _httpClient = new HttpClient();
        }

        [Authorize]
        [HttpGet]
        public IActionResult PaymentProcedure()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PaymentProcedure(PaymentDetails p)
        {
            if (TempData.ContainsKey("VariableName"))
            {
                p.OrderId = TempData["VariableName"].ToString();
            }

            var requestUrl = $"{ApiUrl}/payment";

            // Serialize the Login object to JSON
            var requestData = JsonConvert.SerializeObject(p);

            // Create the HTTP request content with JSON data
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

    }
}
