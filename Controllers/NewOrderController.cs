using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Order.Model;
using System.Net;

namespace Order.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class NewOrderController : ControllerBase
    {

<<<<<<< HEAD

=======
>>>>>>> 718de0af9153a5114d4da90144bba62dc37bf388
        string connectionString = "Data Source=DESKTOP-23TGGIB;Initial Catalog=ProductTable1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";


        [HttpGet("{orderId}")]
        public IActionResult GetOrderDetails(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"SELECT CD.CustomerOrderID, CD.CustomerName, CD.Email, 
                      CA.Street, CA.City, CA.State, CA.ZipCode, 
                      O.TotalAmount, O.TotalTaxAmount,O.TotalOrderAmount
                  FROM CustomerDetails CD
                  INNER JOIN CustomerAddress CA ON CD.CustomerOrderID = CA.OrderID
                  INNER JOIN Orders O ON CD.CustomerOrderID = O.OrderID
                  WHERE CD.CustomerOrderID = @OrderId";

                using (var orderCommand = new SqlCommand(sql, connection))
                {
                    orderCommand.Parameters.AddWithValue("@OrderId", orderId);

                    OrderPlaced order = null;
                    using (var orderReader = orderCommand.ExecuteReader())
                    {
                        if (orderReader.Read())
                        {
                            order = new OrderPlaced
                            {
                                CustomerOrderId = orderReader["CustomerOrderID"].ToString(),
                                CustomerName = orderReader["CustomerName"].ToString(),
                                Email = orderReader["Email"].ToString(),
                                totaltaxamount = Convert.ToDecimal(orderReader["TotalTaxAmount"]),
                                totalamount = Convert.ToDecimal(orderReader["TotalAmount"]),
                                totalorderamount = Convert.ToDecimal(orderReader["TotalOrderAmount"]),

                                customeraddress = new CustomerAddress
                                {
                                    street = orderReader["Street"].ToString(),
                                    city = orderReader["City"].ToString(),
                                    state = orderReader["State"].ToString(),
                                    zip_code = orderReader["ZipCode"].ToString()
                                },
                                OrderedProducts = new List<OrderedProduct>()
                            };
                        }
                    }

                    if (order != null)
                    {
                        // Fetch ordered products along with their prices
                        string productsQuery = @"
                    SELECT OC.ProductId, OC.ProductName, OC.Quantity, PT.Product_Price
                    FROM OrderCart OC
                    INNER JOIN ProductTable PT ON OC.ProductId = PT.Product_Id
                    WHERE OC.CustomerOrderID = @OrderId";

                        using (var productsCommand = new SqlCommand(productsQuery, connection))
                        {
                            productsCommand.Parameters.AddWithValue("@OrderId", orderId);

                            using (var productsReader = productsCommand.ExecuteReader())
                            {
                                while (productsReader.Read())
                                {
                                    var orderedProduct = new OrderedProduct
                                    {
                                        ProductId = Convert.ToInt32(productsReader["ProductId"]),
                                        ProductName = productsReader["ProductName"].ToString(),
                                        Quantity = Convert.ToInt32(productsReader["Quantity"]),
                                        Price = Convert.ToDecimal(productsReader["Product_Price"])
                                    };

                                    // Add the ordered product to the order
                                    order.OrderedProducts.Add(orderedProduct);
                                }
                            }
                        }
                    }

                    return Ok(order);
                }
            }
        }




    }
}