//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using Order.Model;
//using System.Net;

//namespace Order.Controllers
//{
//    [Route("api/orders")]
//    [ApiController]
//    public class OrderController : ControllerBase
//    {

//        string connectionString = "Data Source=DESKTOP-23TGGIB;Initial Catalog=ProductTable1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
     

//        [HttpGet("{orderId}")]
//        public IActionResult GetOrderDetails(int orderId)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                string sql = @"SELECT CD.CustomerOrderID,CD.CustomerName,CD.Email,CA.Street,CA.City,CA.State,CA.ZipCode FROM
//                 CustomerDetails CD  INNER JOIN CustomerAddress CA ON CD.CustomerOrderID = CA.OrderID WHERE  CD.CustomerOrderID =@OrderId";

               

//                    using (var orderCommand = new SqlCommand(sql, connection))
//                {
//                    orderCommand.Parameters.AddWithValue("@OrderId", orderId);

//                    var order = new OrderedPlaced
//                    {
//                        OrderedProducts = new List<OrderedProduct>()
//                    };

//                    using (var orderReader = orderCommand.ExecuteReader())
//                    {
//                        while (orderReader.Read())
//                        {
//                            //order.CustomerOrderId = orderReader["CustomerOrderID"].ToString();
//                            order.CustomerName = orderReader["CustomerName"].ToString();
//                            order.Email = orderReader["Email"].ToString();

//                            order.customeraddress = new CustomerAddress
//                            {
//                                street = orderReader["Street"].ToString(),
//                                city = orderReader["City"].ToString(),
//                                state = orderReader["State"].ToString(),
//                                zip_code = orderReader["ZipCode"].ToString()
//                            };
                         
                           

//                        }
                      
//                    }
//                    return Ok(order);
//                }




//            }
//        }
//    }
//}