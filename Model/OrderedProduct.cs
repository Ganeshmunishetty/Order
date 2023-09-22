using System.Text.Json.Serialization;

namespace Order.Model
{
    public class OrderedProduct
    {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
      
    }
}
