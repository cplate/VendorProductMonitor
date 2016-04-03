using System;

namespace VendorProductMonitor.Monitor
{
    public interface IVendorProductUpdateMonitor
    {
        void Start();
        void Stop();

        event EventHandler<VendorProductsUpdatedEventArgs> VendorProductsUpdated;
    }
}
