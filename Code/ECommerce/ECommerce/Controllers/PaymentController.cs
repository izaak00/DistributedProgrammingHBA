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
            string getOrderJson = TempData["MyObject"] as string;
            Order o = JsonConvert.DeserializeObject<Order>(getOrderJson);

            p.OrderId = o.OrderId;

            // Remove the object from TempData
            TempData.Remove("MyObject");

            // Serialize the Order and Payment object to JSON
            string orderJson = JsonConvert.SerializeObject(o);
            string paymentJson = JsonConvert.SerializeObject(p);
            // Store the objects in TempData
            TempData["PaymentDetails"] = paymentJson;
            TempData["Order"] = orderJson;

            if (orderJson != null & paymentJson != null)
            {
                return RedirectToAction("TriggerCloudFunction", "TriggerHttpFunction");
            }

            return View("Error");
        }

    }
}
