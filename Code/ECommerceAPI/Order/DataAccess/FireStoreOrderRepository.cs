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

        public async Task<List<OrderPaymentShipping>> GetOrderDetails(string email)
        {
            List<OrderPaymentShipping> orderDetailsList = new List<OrderPaymentShipping>();

            // Assuming you have a "users" collection in Firestore
            Query orderQuery = db.Collection("order").WhereEqualTo("Owner", email);

            QuerySnapshot orderQuerySnapshot = await orderQuery.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in orderQuerySnapshot.Documents)
            {
                string orderId = string.Empty;
                string productName = string.Empty;
                string owner = string.Empty;
                string price = string.Empty;
                string address = string.Empty;
                string shippingStatus = string.Empty;

                Dictionary<string, object> getOrderInformation = documentSnapshot.ToDictionary();

                orderId = getOrderInformation.ContainsKey("OrderId") ? getOrderInformation["OrderId"].ToString() : string.Empty;
                owner = getOrderInformation.ContainsKey("Owner") ? getOrderInformation["Owner"].ToString() : string.Empty;
                price = getOrderInformation.ContainsKey("Price") ? getOrderInformation["Price"].ToString() : string.Empty;
                productName = getOrderInformation.ContainsKey("ProductName") ? getOrderInformation["ProductName"].ToString() : string.Empty;

                Query paymentQuery = db.Collection("payment").WhereEqualTo("OrderId", orderId);
                Query shippingQuery = db.Collection("shipping").WhereEqualTo("OrderId", orderId);

                QuerySnapshot paymentQuerySnapshot = await paymentQuery.GetSnapshotAsync();
                QuerySnapshot shippingQuerySnapshot = await shippingQuery.GetSnapshotAsync();

                foreach (DocumentSnapshot paymentSnapshot in paymentQuerySnapshot.Documents)
                {
                    Dictionary<string, object> getPaymentInformation = paymentSnapshot.ToDictionary();
                    address = getPaymentInformation.ContainsKey("Address") ? getPaymentInformation["Address"].ToString() : string.Empty;
                }

                foreach (DocumentSnapshot shippingSnapshot in shippingQuerySnapshot.Documents)
                {
                    Dictionary<string, object> getShippingInformation = shippingSnapshot.ToDictionary();
                    shippingStatus = getShippingInformation.ContainsKey("Status") ? getShippingInformation["Status"].ToString() : string.Empty;
                }

                OrderPaymentShipping orderDetails = new OrderPaymentShipping
                {
                    OrderId = orderId,
                    Owner = owner,
                    Price = price,
                    ProductName = productName,
                    Address = address,
                    ShippingStatus = shippingStatus
                };

                orderDetailsList.Add(orderDetails);
            }

            return orderDetailsList;
        }

    }
}
