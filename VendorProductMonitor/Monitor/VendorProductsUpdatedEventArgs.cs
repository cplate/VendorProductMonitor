using System;
using System.Collections.Generic;

namespace VendorProductMonitor.Monitor
{
    public class VendorProductsUpdatedEventArgs : EventArgs
    {
        public IEnumerable<VendorProductUpdate> Updates { get; set; }

        public VendorProductsUpdatedEventArgs(IEnumerable<VendorProductUpdate> updates)
        {
            Updates = updates;
        }
    }
}
