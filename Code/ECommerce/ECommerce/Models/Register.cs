using Google.Cloud.Firestore;
namespace ECommerce.Models
{
    [FirestoreData]
    public class Register
    {
        private string id;

        [FirestoreProperty]
        public string Id
        {
            get { return id; }
            set { id = Guid.NewGuid().ToString(); }
        }

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
