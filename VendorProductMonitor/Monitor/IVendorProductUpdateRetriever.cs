using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendorProductMonitor.Monitor
{
    public interface IVendorProductUpdateRetriever
    {
        Task<IEnumerable<VendorProductUpdate>> CheckForProductUpdates();
    }
}
