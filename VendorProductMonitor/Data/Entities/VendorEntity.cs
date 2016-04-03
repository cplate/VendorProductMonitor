using Microsoft.WindowsAzure.Storage.Table;

namespace VendorProductMonitor.Data.Entities
{
    public class VendorEntity : TableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
