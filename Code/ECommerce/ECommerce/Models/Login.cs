using Google.Cloud.Firestore;

namespace ECommerce.Models
{
    [FirestoreData]
    public class Login
    {
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }
    }
}
