using Microsoft.AspNetCore.Mvc;
using Payment.DataAccess;
using Payment.Models;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace Payment.Controllers
{
    public class PaymentController : Controller
    {
        FireStorePaymentRepository fspr;

        public PaymentController(FireStorePaymentRepository _fspr)
        {
            fspr = _fspr;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] PaymentDetails p)
        {
            await fspr.AddPayment(p);
            return Ok();
        }
    }
}
