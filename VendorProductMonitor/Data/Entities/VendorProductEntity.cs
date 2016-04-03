using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace VendorProductMonitor.Data.Entities
{
    public class VendorProductEntity : TableEntity
    {
        public Guid ProductId { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
