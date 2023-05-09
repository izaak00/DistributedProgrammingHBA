using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using System.Net;
using Customer.Models;
using Customer.DataAccess;

namespace Customer.Controllers
{
    public class UserController : ControllerBase
    {
        FireStoreUsersRepository fur;

        public UserController(FireStoreUsersRepository _fur)
        {
             fur = _fur;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login l)
        {
            try
            {
                QuerySnapshot snapshot = await fur.Login(l);

                if (snapshot.Documents.Count > 0 && snapshot.Documents[0].GetValue<string>("Password") == l.Password)
                {
                    // Successful login
                    return Ok();
                }
                {
                    return NotFound();
                }
            }
            catch
            {
                // Handle Firestore database errors
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register r)
        {
            try
            {
                QuerySnapshot searchSnapshot = await fur.CheckIfUserExists(r);

                if (searchSnapshot.Count > 0)
                {
                    return Conflict();
                }

                await fur.Register(r);
                return Ok();
            }
            catch
            {
                // Handle Firestore database errors
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
