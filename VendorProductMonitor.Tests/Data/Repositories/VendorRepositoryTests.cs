using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System.Configuration;
using System.Linq;
using VendorProductMonitor.Data.Entities;
using VendorProductMonitor.Data.Repositories;

namespace VendorProductMonitor.Tests.Data.Repositories
{
    public class VendorRepositoryTests
    {
        private CloudTable _table;
        private VendorEntity _v1;

        [SetUp]
        public void Setup()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(ConfigurationManager.AppSettings["VendorTableName"]);

            _table.CreateIfNotExists();

            _v1 = new VendorEntity { Description = "Desc1", Name = "Name1", PartitionKey = "Vendor", RowKey = "V1", Code = "V1" };
            var v2 = new VendorEntity { Description = "Desc2", Name = "Name2", PartitionKey = "Vendor", RowKey = "V2", Code = "V2" };
            var v3 = new VendorEntity { Description = "Desc3", Name = "Name3", PartitionKey = "Vendor", RowKey = "V3", Code = "V3" };
            TableOperation insertV1 = TableOperation.Insert(_v1);
            _table.Execute(insertV1);
            TableOperation insertV2 = TableOperation.Insert(v2);
            _table.Execute(insertV2);
            TableOperation insertV3 = TableOperation.Insert(v3);
            _table.Execute(insertV3);
        }

        [TearDown]
        public void Purge()
        {
            _table.DeleteIfExists();
        }

        [TestCase]
        public void GetAll_Successful()
        {
            var target = new VendorRepository();
            var result = target.GetAll().Result;
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.That(3, Is.EqualTo(resultList.Count));
            var v1 = resultList.FirstOrDefault(x => x.Code == "V1");
            Assert.IsNotNull(v1);
            Assert.That(v1.Description, Is.EqualTo(_v1.Description));
            Assert.That(v1.Name, Is.EqualTo(_v1.Name));
            Assert.That(v1.Code, Is.EqualTo(_v1.Code));
        }
    }
}
