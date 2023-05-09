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
    }
}
