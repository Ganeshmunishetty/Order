
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Order.Model;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Order.Controllers
{

    [Route("api/Products")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-23TGGIB;Initial Catalog=ProductTable1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

<<<<<<< HEAD
        private readonly IValidator<CustomerOrderDetails> _validate;
        private readonly IConfiguration _config;

        public ProductController(IValidator<CustomerOrderDetails> validator, IConfiguration config)
        {
            _validate = validator;
            _config = config;
        }
=======
>>>>>>> 718de0af9153a5114d4da90144bba62dc37bf388
        [Route("GetAllProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select * from ProductTable";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Product_Id"];
                            string name = reader["Product_Name"].ToString();
                            int price =(int) reader["Product_Price"];

                            products.Add(new Product { product_id = id, product_name = name, product_price = price });
                        }
                    }
                }
            }
            return Ok(products);
        }

        [Route("CreateOrder")]
        [HttpPost]
<<<<<<< HEAD
        public async  Task<IActionResult> CreateOrder([FromBody] CustomerOrderDetails orderDetails)
=======
        public IActionResult CreateOrder([FromBody] CustomerOrderDetails orderDetails)
>>>>>>> 718de0af9153a5114d4da90144bba62dc37bf388
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
<<<<<<< HEAD
                var validationResult = await _validate.ValidateAsync(orderDetails);
=======
>>>>>>> 718de0af9153a5114d4da90144bba62dc37bf388

                int orderId = 1;
                int rowsAffected = 0;

                // Insert customer details
                string insertCustomerQuery = "INSERT INTO CustomerDetails (CustomerName, Email) VALUES (@CustomerName, @Email); SELECT SCOPE_IDENTITY();";
                using (SqlCommand insertCustomerCommand = new SqlCommand(insertCustomerQuery, connection))
                {
                    insertCustomerCommand.Parameters.AddWithValue("@CustomerName", orderDetails.customer_name);
                    insertCustomerCommand.Parameters.AddWithValue("@Email", orderDetails.email);
                    orderId = Convert.ToInt32(insertCustomerCommand.ExecuteScalar());
                }

                // Insert customer address
                string insertAddressQuery = "INSERT INTO CustomerAddress ( Street, City, State, ZipCode) VALUES ( @Street, @City, @State, @ZipCode);";
                using (SqlCommand insertAddressCommand = new SqlCommand(insertAddressQuery, connection))
                {
                    //insertAddressCommand.Parameters.AddWithValue("@OrderId", orderId);
                    insertAddressCommand.Parameters.AddWithValue("@Street", orderDetails.customerAddress.street);
                    insertAddressCommand.Parameters.AddWithValue("@City", orderDetails.customerAddress.city);
                    insertAddressCommand.Parameters.AddWithValue("@State", orderDetails.customerAddress.state);
                    insertAddressCommand.Parameters.AddWithValue("@ZipCode", orderDetails.customerAddress.zip_code);
                    rowsAffected = insertAddressCommand.ExecuteNonQuery();
                }

                // Calculate order total, tax, and total order amount
                decimal orderTotal = 0;
                decimal totalTaxAmount = 0;

                foreach (var item in orderDetails.ordercart)
                {
                    string getProductPriceQuery = "SELECT Product_Price FROM ProductTable WHERE Product_Id = @Product_Id";
                    using (SqlCommand getPriceCommand = new SqlCommand(getProductPriceQuery, connection))
                    {
                        getPriceCommand.Parameters.AddWithValue("@Product_Id", item.product_id);
                        string priceString = getPriceCommand.ExecuteScalar()?.ToString();

                        if (decimal.TryParse(priceString, out decimal productPrice))
                        {
                            // Calculate subtotal for each product
                            decimal subtotal = productPrice * item.Quantity;
                            orderTotal += subtotal;
                        }
                    }
                }

                // Calculate total tax amount and total order amount
                double taxPerRupee = 0.18;
                totalTaxAmount = orderTotal * (decimal)taxPerRupee;
                decimal totalOrderAmount = totalTaxAmount + orderTotal;

                // Insert order details
                string insertOrderQuery = "INSERT INTO Orders (OrderID, TotalAmount, TotalTaxAmount, TotalOrderAmount) VALUES (@OrderID, @TotalAmount, @TotalTaxAmount, @TotalOrderAmount)";
                using (SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, connection))
                {
                    insertOrderCommand.Parameters.AddWithValue("@OrderID", orderId);
                    insertOrderCommand.Parameters.AddWithValue("@TotalAmount", orderTotal);
                    insertOrderCommand.Parameters.AddWithValue("@TotalTaxAmount", totalTaxAmount);
                    insertOrderCommand.Parameters.AddWithValue("@TotalOrderAmount", totalOrderAmount);
                    rowsAffected += insertOrderCommand.ExecuteNonQuery();
                }

                // Insert products into ProductTable
                string insertProductQuery = "INSERT INTO OrderCart (CustomerOrderID,ProductId, ProductName, Quantity) VALUES (@CustomerOrderID,@ProductId, @ProductName, @Quantity)";
                using (SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connection))
                {
                    foreach (var item in orderDetails.ordercart)
                    {
                        // Access data from ordercart
                        int productId = item.product_id;
                        string productName = item.product_name;
                        int quantity = item.Quantity;

                        // Insert ordercart data into ProductTable
                        insertProductCommand.Parameters.Clear();
                        insertProductCommand.Parameters.AddWithValue("@CustomerOrderID", orderId);
                        insertProductCommand.Parameters.AddWithValue("@ProductId", productId);
                        insertProductCommand.Parameters.AddWithValue("@ProductName", productName);
                        insertProductCommand.Parameters.AddWithValue("@Quantity", quantity);

                        rowsAffected += insertProductCommand.ExecuteNonQuery();
                    }

                }

                connection.Close();

                // Return the order details
                return Ok(new
                {
                    OrderId = orderId,
                    TotalAmount = orderTotal,
                    TotalTaxAmount = totalTaxAmount,
                    TotalOrderAmount = totalOrderAmount
                });
            }
        }

        [Route("UpdateProduct")]
        [HttpPut]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string update = "Update OrderCart Set Product_Name=@Product_Name,Product_Price=@Product_Price where Product_Id=@Product_ID";
                using (SqlCommand command = new SqlCommand(update, conn))
                {
                    command.Parameters.AddWithValue("@Product_ID", product.product_id);
                    command.Parameters.AddWithValue("@Product_Name", product.product_name);
                    command.Parameters.AddWithValue("@Product_Price", product.product_price);

                    int rowsaffected = command.ExecuteNonQuery();
                    return Ok(rowsaffected);
                }
            }
        }

        [Route("DeleteProductFromId")]
        [HttpDelete]
        public IActionResult DeleteProductById(int product_Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string delete = "delete from OrderCart where Product_Id=@Product_Id";
                using (SqlCommand command = new SqlCommand(delete, connection))
                {
                    command.Parameters.AddWithValue("@Product_ID", product_Id);

                    int rowsaffected = command.ExecuteNonQuery();
                    return Ok(rowsaffected);
                }
            }
        }

        //[Route("DeleteProduct")]
        //[HttpDelete]
        //public IActionResult DeleteProduct([FromQuery] Product product)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string delete = "delete from OrderCart where Product_Id=@Product_Id";
        //        using (SqlCommand command = new SqlCommand(delete, connection))
        //        {
        //            command.Parameters.AddWithValue("@Product_ID", product.product_id);

        //            int rowsaffected = command.ExecuteNonQuery();
        //            return Ok(rowsaffected);
        //        }
        //    }
        //}

    }
}
