using System;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Models;

namespace VendorProductMonitor.Data.Repositories
{
    public interface IVendorProductRepository
    {
        Task<VendorProduct> Get(string vendorCode, Guid productId);
    }
}
