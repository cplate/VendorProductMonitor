using System;

namespace VendorProductMonitor.Data.Models
{
    public class VendorProduct
    {
        public Guid ProductId { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
