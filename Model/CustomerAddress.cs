using System.Text.Json.Serialization;

namespace Order.Model
{
    public class CustomerAddress
    {
        //[JsonIgnore]
        //public string OrderId { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
    }
}
