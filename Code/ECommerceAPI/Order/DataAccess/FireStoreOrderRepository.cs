using Google.Cloud.Firestore;
using Order.Models;

namespace Order.DataAccess
{
    public class FireStoreOrderRepository
    {
        FirestoreDb db;

        public FireStoreOrderRepository(string project)
        {
            db = FirestoreDb.Create(project);
        }

        public async Task AddOrder(Orders o)
        {
            CollectionReference orderCollection = db.Collection("order");

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "OrderId", o.OrderId },
                    { "ProductName", o.ProductName },
                    { "Price", o.Price },
                    { "Owner", o.Owner },
                };

            await orderCollection.AddAsync(data);
        }

    }
}
