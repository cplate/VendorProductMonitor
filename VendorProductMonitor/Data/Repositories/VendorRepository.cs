using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Entities;
using VendorProductMonitor.Data.Models;

namespace VendorProductMonitor.Data.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        public async Task<IEnumerable<Vendor>> GetAll()
        {
            List<Vendor> allVendors = new List<Vendor>();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(ConfigurationManager.AppSettings["VendorTableName"]);

            TableQuery<VendorEntity> query = new TableQuery<VendorEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Vendor"));
            TableContinuationToken continuationToken = null;

            do
            {
                TableQuerySegment<VendorEntity> tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = tableQueryResult.ContinuationToken;

                var entities = tableQueryResult.Results;
                allVendors.AddRange(entities.Select(x => new Vendor { Name = x.Name, Code = x.Code, Description = x.Description }));

            } while (continuationToken != null);
            
            return allVendors;
        }
    }
}
