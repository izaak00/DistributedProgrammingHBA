using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HttpFunction;

public class Function : IHttpFunction
{
    /// <summary>
    /// Logic for your function goes here.
    /// </summary>
    /// <param name="context">The HTTP context, containing the request and the response.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private readonly ILogger<Function> _logger;
     private readonly HttpClient _httpClient;
    private const string paymentApiUrl = "https://paymentapiimage-j7lgba4epq-uc.a.run.app";
    private const string orderApiUrl = "https://orderapiimage-j7lgba4epq-uc.a.run.app";
    private const string shippingApiUrl = "https://shippingapiimage-j7lgba4epq-uc.a.run.app";
    public Function(ILogger<Function> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task HandleAsync(HttpContext context)
    {
        try
        {
            string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // Deserialize the payment details object
            PaymentDetails paymentDetails = JsonConvert.DeserializeObject<PaymentDetails>(requestBody);

            // Retrieve the order JSON from the custom header
            string orderJson = context.Request.Headers["X-Order-Json"];

            // Deserialize the order object
            Order order = JsonConvert.DeserializeObject<Order>(orderJson);

            await OrderDetails(order);
            await PaymentProcedure(paymentDetails);

            ShippingDetails shipping = new ShippingDetails
            {
                OrderId = paymentDetails.OrderId,
                Address = paymentDetails.Address,
                ShippingStatus = "Order received and not yet dispached"
            };

            await ShippingDetails(shipping);
            // Process the payment details and order objects
            // ...

            await context.Response.WriteAsync("Hello, Functions Framework.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred");
            context.Response.StatusCode = 500;
        }
    }

    [HttpPost]
    public async Task PaymentProcedure(PaymentDetails p)
    {
        var requestUrl = $"{paymentApiUrl}/payment";

        // Serialize the Login object to JSON
        var requestData = JsonConvert.SerializeObject(p);

        // Create the HTTP request content with JSON data
        var content = new StringContent(requestData, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("[Payment] Successfully added information to database");
        }
        else 
        {
            _logger.LogError("[Payment] An error occured when adding information to database");
        }           
    }

    [HttpPost]
    public async Task OrderDetails(Order o)
    {
        var requestUrl = $"{orderApiUrl}/order";

        // Serialize the Login object to JSON
        var requestData = JsonConvert.SerializeObject(o);

        // Create the HTTP request content with JSON data
        var content = new StringContent(requestData, Encoding.UTF8, "application/json");

        // Make a GET request to the API to retrieve user details
        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("[Order] Successfully added information to database");
        }
        else 
        {
            _logger.LogError("[Order] An error occured when adding information to database");
        }       
    }

    [HttpPost]
    public async Task ShippingDetails(ShippingDetails s)
    {
        var requestUrl = $"{shippingApiUrl}/shipping";

        // Serialize the Login object to JSON
        var requestData = JsonConvert.SerializeObject(s);

        // Create the HTTP request content with JSON data
        var content = new StringContent(requestData, Encoding.UTF8, "application/json");

        // Make a GET request to the API to retrieve user details
        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("[Shipping] Successfully added information to database");
        }
        else 
        {
            _logger.LogError("[Shipping] An error occured when adding information to database");
        }       
    }
}
