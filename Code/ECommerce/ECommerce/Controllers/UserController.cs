using ECommerce.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserController> Logger;
        private const string ApiUrl = "https://localhost:7166";

        public UserController(ILogger<UserController> logger)
        {
            _httpClient = new HttpClient();
            Logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        { 
            return View();
        }

        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost] 
        public async Task<IActionResult> Login(Login l)
        {
            var requestUrl = $"{ApiUrl}/login";

            // Serialize the Login object to JSON
            var requestData = JsonConvert.SerializeObject(l);

            // Create the HTTP request content with JSON data
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // User is logged in successfully
                // You can store the authentication token or perform other actions here

                // Remove the login and register cookies

                // Create the claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, l.Email),
                    // Add additional claims as needed
                };
                // Create the ClaimsIdentity
                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true
                });

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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Perform the logout process

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
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

            // Serialize the Login object to JSON
            var requestData = JsonConvert.SerializeObject(r);

            // Create the HTTP request content with JSON data
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            string email = User.Identity.Name;
            var requestUrl = $"{ApiUrl}/userdetails/{email}";

            // Make a GET request to the API to retrieve user details
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                UserDetails userDetails = await response.Content.ReadFromJsonAsync<UserDetails>();
                return View(userDetails);
            }

            return View("Error");
        }
    }
}
