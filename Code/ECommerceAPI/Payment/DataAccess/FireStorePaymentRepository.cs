using Payment.Models;
using Google.Cloud.Firestore;

namespace Payment.DataAccess
{
    public class FireStorePaymentRepository
    {
        FirestoreDb db;

        public FireStorePaymentRepository(string project)
        {
            db = FirestoreDb.Create(project);
        }

        public async Task AddPayment(PaymentDetails p)
        {
            CollectionReference paymentCollection = db.Collection("payment");

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "OrderId", p.OrderId }, 
                    { "CardType", p.CardType },
                    { "Currency", p.Currency },
                    {"CardNumber", p.CardNumber }
                };
            await paymentCollection.AddAsync(data);
        }
    }
}
