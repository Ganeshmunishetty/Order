using System.Text.Json.Serialization;

namespace Order.Model
{
    public class CustomerOrderDetails
    {
      
        //public int CustomerOrderId { get; set; }
        public string customer_name { get; set; }
        public string email { get; set; }
        public CustomerAddress customerAddress { get; set; }
        public List<OrderCart> ordercart { get; set; }
   
    }
}
