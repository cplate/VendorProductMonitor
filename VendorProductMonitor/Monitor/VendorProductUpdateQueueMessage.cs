using System.Collections.Generic;

namespace VendorProductMonitor.Monitor
{
    public class VendorProductUpdateQueueMessage
    {
        public List<VendorProductUpdate> Updates { get; set; }
    }
}
