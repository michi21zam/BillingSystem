using System;
using System.Collections.Generic;

namespace BillingSystem.Models
{
    public class InvoiceIndexViewModel
    {
        public IEnumerable<Invoice> Invoices { get; set; }

        // Filters
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? TotalAmount { get; set; }

        // Paging
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
