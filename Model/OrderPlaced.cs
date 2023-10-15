using System.Net;

namespace Order.Model
{
    public class OrderPlaced
    {
        public string CustomerOrderId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public decimal totaltaxamount { get; set; }
        public decimal totalamount { get; set; }
        public decimal totalorderamount { get; set; }
        public CustomerAddress customeraddress { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }

      


    }
}
