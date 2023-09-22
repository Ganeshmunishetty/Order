
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Order.Model;
using System.Net;

namespace Order.Controllers
{

    [Route("api/Products")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-23TGGIB;Initial Catalog=ProductTable1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
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
                            int price = (int)reader["Product_Price"];

                            products.Add(new Product { product_id = id, product_name = name, product_price = price });
                        }
                    }
                }
            }
            return Ok(products);
        }

        [Route("AddProduct")]
        [HttpPost]
        public IActionResult AddProducts([FromBody] OrderDetails orderdetails)
        {

            //List<Product> products = new List<Product>();
            //for (int i = 0; i < 3; i++)
            //{
            //    products[i].product_id = i;
            //    products[i].product_name = "earphone" + i.ToString();
            //    products[i].product_price = 200 + i;
            //}


            foreach (var item in orderdetails.productcart)
            {
                var productId = item.product_id;
                var productName = item.product_name;
                var productPrice = item.Quantity;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int rowsaffected = 0;
                string sql = "insert into CustomerOrderdetails (CustomerName,Email,TaxPercentage,TotalTaxAmount,TotalAmount) values(@CustomerName,@Email,@TaxPercentage,@TotalTaxAmount,@TotalAmount)";
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@CustomerName", orderdetails.customer_name);
                    command.Parameters.AddWithValue("@Email", orderdetails.email);
                    command.Parameters.AddWithValue("@TaxPercentage", orderdetails.tax_percentage);
                    command.Parameters.AddWithValue("@TotalTaxAmount", orderdetails.total_tax_amount);
                    command.Parameters.AddWithValue("@TotalAmount", orderdetails.total_amount);

                    rowsaffected = command.ExecuteNonQuery();
                }
                string sql1 = "insert into CustomerAddress (Street,City,State,ZipCode) values(@Street,@City,@State,@ZipCode)";
                using (SqlCommand cmd1 = new SqlCommand(sql1, conn))
                {

                    cmd1.Parameters.AddWithValue("@Street", orderdetails.customerAddress.street);
                    cmd1.Parameters.AddWithValue("@City", orderdetails.customerAddress.city);
                    cmd1.Parameters.AddWithValue("@State", orderdetails.customerAddress.state);
                    cmd1.Parameters.AddWithValue("@ZipCode", orderdetails.customerAddress.zip_code);

                    rowsaffected = cmd1.ExecuteNonQuery();
                }


                string sqlproduct = "INSERT INTO OrderedProducts (ProductId, ProductName, Quantity) VALUES ";
                string valueClause = string.Join(",", orderdetails.productcart.Select(p =>
                {
                    string productIdParam = $"@ProductId{p.product_id}";
                    string productNameParam = $"@ProductName{p.product_name}";
                    string quantityParam = $"@Quantity{p.Quantity}";
                    return $"({productIdParam}, {productNameParam}, {quantityParam})";
                }));

                sqlproduct += valueClause;


                using (SqlCommand cmd1 = new SqlCommand(sqlproduct, conn))
                {
                    foreach (var item in orderdetails.productcart)
                    {
                        cmd1.Parameters.AddWithValue($"@ProductId{item.product_id}", item.product_id);
                        cmd1.Parameters.AddWithValue($"@ProductName{item.product_name}", item.product_name);
                        cmd1.Parameters.AddWithValue($"@Quantity{item.Quantity}", item.Quantity);
                    }

                    //foreach (var item in orderdetails.productcart)
                    //{
                    //    cmd1.Parameters.AddWithValue($"@ProductId" + item.product_id, item.product_id);
                    //}

                                                                                                   

                     rowsaffected = cmd1.ExecuteNonQuery();
                }



                return Ok(rowsaffected);
            }
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] Product product, [FromQuery] int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string update = "Update ProductTable Set Product_Name=@Product_Name,Product_Price=@Product_Price where Product_Id=@Product_ID";
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

        [HttpDelete]
        public IActionResult DeleteProduct([FromQuery] Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string delete = "delete from ProductTable where Product_Id=@Product_Id";
                using (SqlCommand command = new SqlCommand(delete, connection))
                {
                    command.Parameters.AddWithValue("@Product_ID", product.product_id);

                    int rowsaffected = command.ExecuteNonQuery();
                    return Ok(rowsaffected);
                }
            }
        }
    }
}
