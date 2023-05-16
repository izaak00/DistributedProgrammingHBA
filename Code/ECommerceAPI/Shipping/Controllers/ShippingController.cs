using Microsoft.AspNetCore.Mvc;
using Shipping.DataAccess;
using Shipping.Models;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace Shipping.Controllers
{
    public class ShippingController : Controller
    {

        FireStoreShippingRepository fsr;

        public ShippingController(FireStoreShippingRepository _fsr)
        {
            fsr = _fsr;
        }

        [HttpPost("shipping")]
        public async Task<IActionResult> Shipping([FromBody] ShippingDetails s)
        {
            await fsr.AddShipping(s);
            return Ok();
        }

        [HttpPost("updateshippingstatus")]
        public async Task<IActionResult> UpdateShippingStatus(string status, string orderId)
        {
            await fsr.UpdateStatus(status,orderId);
            return Ok();
        }
    }
}
