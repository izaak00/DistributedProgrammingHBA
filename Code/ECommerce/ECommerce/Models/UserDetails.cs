using Google.Cloud.Firestore;

namespace ECommerce.Models
{
    [FirestoreData]
    public class UserDetails
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Surname { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }
    }
}
