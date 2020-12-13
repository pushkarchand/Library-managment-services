using System;
using System.Collections.Generic;

#nullable disable

namespace Gateway.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string CreditCard { get; set; }
        public DateTime OrderDateAndTime { get; set; }
        public string BookCode { get; set; }
        public string UserCode { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
