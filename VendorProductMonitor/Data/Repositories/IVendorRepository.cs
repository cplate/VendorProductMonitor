using System.Collections.Generic;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Models;

namespace VendorProductMonitor.Data.Repositories
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAll();
    }
}
