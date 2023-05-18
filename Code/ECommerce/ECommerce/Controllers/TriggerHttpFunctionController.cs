using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace ECommerce.Controllers
{
    public class TriggerHttpFunctionController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string HttpFunctionUrl = "https://us-central1-swd63aprogrammingforthecloud.cloudfunctions.net/http-function-distributedhba";


        public TriggerHttpFunctionController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> TriggerCloudFunction()
        {
            try
            {
                string paymentDetailsJson = TempData["PaymentDetails"] as string;
                string orderJson = TempData["Order"] as string;

                // Remove the object from TempData
                TempData.Remove("PaymentDetails");
                TempData.Remove("Order");
 

                var request = new HttpRequestMessage(HttpMethod.Post, HttpFunctionUrl)
                {
                    Content = new StringContent(paymentDetailsJson, Encoding.UTF8, "application/json")
                };

                // Add the order JSON as a custom header
                request.Headers.Add("X-Order-Json", orderJson);

                // Send the request and get the response
                var response = await _httpClient.SendAsync(request);

                // Handle the response as needed
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // ...
                return StatusCode(500);
            }
        }
    }
}
