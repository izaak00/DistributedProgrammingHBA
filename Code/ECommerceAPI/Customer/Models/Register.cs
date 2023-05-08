using Google.Cloud.Firestore;

namespace Customer.Models
{
    public class Register
    {
        public string Id {get; set;}

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
