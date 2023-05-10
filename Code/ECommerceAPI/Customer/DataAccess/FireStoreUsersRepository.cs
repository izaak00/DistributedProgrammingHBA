using Customer.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace Customer.DataAccess
{
    public class FireStoreUsersRepository
    {
        FirestoreDb db;

        public FireStoreUsersRepository(string project)
        {
            db = FirestoreDb.Create(project);
        }

        public async Task<QuerySnapshot> Login(Login l)
        {
            CollectionReference usersCollection = db.Collection("users");
            Query searchExistingUser = usersCollection.WhereEqualTo("Email", l.Email);
            QuerySnapshot snapshot = await searchExistingUser.GetSnapshotAsync();

            return snapshot;
        }

        public async Task<QuerySnapshot> CheckIfUserExists(Register r)
        {
            CollectionReference usersCollection = db.Collection("users");
            Query searchExistingUser = usersCollection.WhereEqualTo("email", r.Email);
            QuerySnapshot snapshot = await searchExistingUser.GetSnapshotAsync();

            return snapshot;
        }

        public async Task Register(Register r)
        {
            CollectionReference usersCollection = db.Collection("users"); // Assuming you have a document ID generation strategy

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Name", r.Name },
                    { "Surname", r.Surname },
                    { "Email", r.Email },
                    { "Password", r.Password },
                };

            await usersCollection.AddAsync(data);
        }

        public async Task<UserDetails> GetUserDetails(string email)
        {
            // Assuming you have a "users" collection in Firestore
            Query userQuery = db.Collection("users").WhereEqualTo("Email", email);
            QuerySnapshot userQuerySnapshot = await userQuery.GetSnapshotAsync();

            string name = string.Empty;
            string surname = string.Empty;

            foreach (DocumentSnapshot documentSnapshot in userQuerySnapshot.Documents)
            {
                Dictionary<string, object> getUserInformation = documentSnapshot.ToDictionary();

                name = getUserInformation.ContainsKey("Name") ? getUserInformation["Name"].ToString() : string.Empty;
                surname = getUserInformation.ContainsKey("Surname") ? getUserInformation["Surname"].ToString() : string.Empty;

                // Assuming there is only one matching document, you can break the loop here
                break;
            }

            // Create a new UserDetails object with the extracted fields
            UserDetails userDetails = new UserDetails
            {
                Email = email, // Assign the provided email to the Email property, not userDetails
                Name = name,
                Surname = surname
            };

            return userDetails;
        }
    }
}
