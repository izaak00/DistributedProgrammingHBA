using Google.Cloud.Firestore;

namespace Customer.Models
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
    }
}
