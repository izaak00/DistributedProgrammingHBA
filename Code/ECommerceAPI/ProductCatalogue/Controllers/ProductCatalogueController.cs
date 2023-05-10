using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProductCatalogue.Models;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata.Ecma335;

namespace ProductCatalogue.Controllers
{
    public class ProductCatalogueController : Controller
    {
        private async Task<JsonDocument> GetResponse(string requestURI)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestURI),
                Headers =
            {
                { "X-RapidAPI-Key", "f3bb99dfcamsh6ae6f5fb7ff016bp105f74jsn092e52348300" },
                { "X-RapidAPI-Host", "ebay-data-scraper.p.rapidapi.com" },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(body);
            }
        }

        [HttpGet("GetProduct/{productName}")]

        public async Task<List<Product>> GetProduct(string productName)
        {
            string uri = $"https://ebay-data-scraper.p.rapidapi.com/products?page_number=1&product_name={productName}&country=canada";

            JsonDocument response = await GetResponse(uri);

            if (response.RootElement.ValueKind == JsonValueKind.Array)
            {
                List<Product> products = new List<Product>();

                foreach (JsonElement productElement in response.RootElement.EnumerateArray())
                {
                    if (productElement.ValueKind == JsonValueKind.Object)
                    {
                        Product product = new Product
                        {
                            name = productElement.GetProperty("name").GetString(),
                            price = productElement.GetProperty("price").GetString(),
                            condition = productElement.GetProperty("condition").GetString(),
                            thumbnail = productElement.GetProperty("thumbnail").GetString()
                        };

                        products.Add(product);
                    }
                }

                return products;
            }

            return null;
        }
    }
}
