using Google.Cloud.Firestore;
using Shipping.Models;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace Shipping.DataAccess
{
    public class FireStoreShippingRepository
    {
        FirestoreDb db;


        public FireStoreShippingRepository(string project)
        {
            db = FirestoreDb.Create(project);
        }

        public async Task AddShipping(ShippingDetails s)
        {
            CollectionReference orderCollection = db.Collection("shipping");

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "OrderId", s.OrderId },
                    { "Status", s.ShippingStatus },
                    { "Address", s.Address }
                };

            await orderCollection.AddAsync(data);
        }

        public async Task UpdateStatus(string status, string orderId)
        {
            string documentId = await GetShippingDocumentId(orderId);

            DocumentReference shippingRef = db.Collection("shipping").Document(documentId);

            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Status", status }
            };

            await shippingRef.UpdateAsync(updates);
        }

        public async Task<string> GetShippingDocumentId(string orderId)
        {
            Query allShippingQuery = db.Collection("shipping").WhereEqualTo("OrderId", orderId);
            QuerySnapshot allShippingQuerySnapshot = await allShippingQuery.GetSnapshotAsync();

            DocumentSnapshot documentSnapshot = allShippingQuerySnapshot.Documents.FirstOrDefault();
            return documentSnapshot.Id;
        }
    }
}
