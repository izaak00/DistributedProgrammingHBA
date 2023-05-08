using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://your-api-url.com/users";

        public UserController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Login()
        { 
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Login(Login l)
        {
            var requestUrl = $"{ApiUrl}/login";
            var requestData = new { Email = l.Email, Password = l.Password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                // User is logged in successfully
                // You can store the authentication token or perform other actions here
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // User does not exist
                TempData["ErrorMessage"] = "User does not exist.";
            }
            else
            {
                // Handle other error cases
                TempData["ErrorMessage"] = "An error occurred.";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register r)
        {
            var requestUrl = $"{ApiUrl}/register";
            var requestData = new { ID = r.Id, Name = r.Name, Surname = r.Surname, Email = r.Email, Password = r.Password };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, requestData);

            if (response.IsSuccessStatusCode)
            {
                // User is signed up successfully
                return RedirectToAction("Login", "User");
            }
            else
            {
                // Handle error case
                TempData["ErrorMessage"] = "An error occurred during sign-up.";
            }

            return View();
        }
    }
}
