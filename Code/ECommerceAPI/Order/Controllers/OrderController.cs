﻿using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Order.DataAccess;
using Order.Models;
using System.Net;

namespace Order.Controllers
{
    public class OrderController : ControllerBase
    {
        FireStoreOrderRepository fsor;

        public OrderController(FireStoreOrderRepository _fsor)
        {
            fsor = _fsor;
        }

        [HttpPost("order")]
        public async Task<IActionResult> Order([FromBody] Orders o)
        {
            await fsor.AddOrder(o);
            return Ok();
        }

        [HttpGet("orderpaymentshipping/{email}")]

        public async Task<ActionResult<List<OrderPaymentShipping>>> GetOrderDetails(string email)
        {
            List<OrderPaymentShipping> orderPaymentShippingList = await fsor.GetOrderDetails(email);

            if (orderPaymentShippingList == null || orderPaymentShippingList.Count == 0)
            {
                return NotFound(); // Return a 404 Not Found response
            }
            return orderPaymentShippingList;
        }
    }
}
