using System;
using System.Collections.Generic;

#nullable disable

namespace LoanMS.Models
{
    public partial class Loan
    {
        public int LoanId { get; set; }
        public string BorrowingDate { get; set; }
        public string ReturnDate { get; set; }
        public string UserCode { get; set; }
        public string BookCode { get; set; }
    }
}
