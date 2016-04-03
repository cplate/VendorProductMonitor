using System;
using System.Collections.Generic;
using VendorProductMonitor.Data.Models;
using VendorProductMonitor.ViewModels;

namespace VendorProductMonitor.Views
{
    public interface IVendorProductUpdatesView
    {
        void SetVendorList(IEnumerable<Vendor> vendors);
        void SetProductUpdateList(IEnumerable<VendorProductUpdateViewModel> updatedProducts);
        void Show();

        event EventHandler<EventArgs> StopMonitoring;
    }
}
