using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Entities;
using VendorProductMonitor.Data.Models;

namespace VendorProductMonitor.Data.Repositories
{
    public class VendorProductRepository : IVendorProductRepository
    {
        public async Task<VendorProduct> Get(string vendorCode, Guid productId)
        {            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();            
            CloudTable table = tableClient.GetTableReference(ConfigurationManager.AppSettings["VendorProductTableName"]);
            
            TableOperation retrievalOperation = TableOperation.Retrieve<VendorProductEntity>(vendorCode, $"Product__{productId}");
            
            TableResult retrievalResult = await table.ExecuteAsync(retrievalOperation);

            if (retrievalResult?.Result != null)
            {
                var entity = (VendorProductEntity)retrievalResult.Result;
                return new VendorProduct { VendorCode = entity.VendorCode, Name = entity.Name, Description = entity.Description, ProductId = entity.ProductId, Price = entity.Price };
            }
            return null;
        }
    }
}
