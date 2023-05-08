using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using System.Net;
using Customer.Models;

namespace Customer.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly FirestoreDb _db;
        private const string CollectionName = "users";

        public UserController()
        {
            // Initialize Firestore database connection
            // Replace "your-project-id" with your actual Firestore project ID
            string projectId = "your-project-id";
            FirestoreDbBuilder builder = new FirestoreDbBuilder
            {
                ProjectId = projectId
            };
            _db = builder.Build();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login l)
        {
            try
            {
                DocumentReference docRef = _db.Collection(CollectionName).Document(l.Email);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists && snapshot.GetValue<string>("password") == l.Password)
                {
                    return Ok();
                }
                else
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
        public async Task<IActionResult> Register(Register r)
        {
            try
            {
                CollectionReference usersCollection = _db.Collection("users");
                Query searchExistingUser = usersCollection.WhereEqualTo("Email", r.Email);
                QuerySnapshot searchSnapshot = await searchExistingUser.GetSnapshotAsync();

                if (searchSnapshot.Count > 0)
                {
                    return Conflict();
                }

                DocumentReference docRef = usersCollection.Document(); // Assuming you have a document ID generation strategy

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Name", r.Name },
                    { "Surname", r.Surname },
                    { "Email", r.Email },
                    { "Password", r.Password },
                };

                await docRef.SetAsync(data);

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
