namespace Order.Model
{
    public class OrderDetails
    {
        public string customer_name { get; set; }
        public string email { get; set; }
        public int tax_percentage { get; set; }
        public int total_tax_amount { get; set; }
        public int total_amount { get; set; }
        public CustomerAddress customerAddress { get; set; }
        public List<Product> product { get; set; }
    }
}
