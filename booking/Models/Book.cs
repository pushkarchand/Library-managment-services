using System;
using System.Collections.Generic;

#nullable disable

namespace BookingMS.Models
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int CatId { get; set; }
        public string BookCode { get; set; }
        public double Price { get; set; }

        public virtual Category Cat { get; set; }
    }
}
