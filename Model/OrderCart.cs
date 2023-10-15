using System.Text.Json.Serialization;

namespace Order.Model
{
    public class OrderCart
    {
        //[JsonIgnore]
        //public int OrderId { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int Quantity { get; set; }
    }
}
