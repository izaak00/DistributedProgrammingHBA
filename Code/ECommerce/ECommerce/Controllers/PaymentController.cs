using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Models;
using Newtonsoft.Json;
using static Humanizer.On;
using System.Net.Http;
using System.Text;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using Order = ECommerce.Models.Order;

namespace ECommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7114";

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
            string orderJson = TempData["MyObject"] as string;

            // Remove the object from TempData
            TempData.Remove("MyObject");

            // Serialize the Order object to JSON
            string paymentJson = JsonConvert.SerializeObject(p);
            // Store the objects in TempData
            TempData["PaymentDetails"] = paymentJson;
            TempData["Order"] = orderJson;

            if (orderJson != null & paymentJson != null)
            {
                return RedirectToAction("TriggerCloudFunction", "TriggerHttpFunction");
            }

            return View("Error");

            //var requestUrl = $"{ApiUrl}/payment";

            //// Serialize the Login object to JSON
            //var requestData = JsonConvert.SerializeObject(p);

            //// Create the HTTP request content with JSON data
            //var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //return View("Error");
        }

    }
}
