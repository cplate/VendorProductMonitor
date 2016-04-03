using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System;
using System.Configuration;
using VendorProductMonitor.Data.Entities;
using VendorProductMonitor.Data.Repositories;

namespace VendorProductMonitor.Tests.Data.Repositories
{
    [TestFixture]
    public class VendorProductRepositoryTests
    {
        private CloudTable _table;
        private VendorProductEntity _p1;

        [SetUp]
        public void Setup()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(ConfigurationManager.AppSettings["VendorProductTableName"]);

            _table.CreateIfNotExists();

            var p1Id = Guid.NewGuid();
            var p1Vendor = "V1";
            _p1 = new VendorProductEntity { Price = 5.49, Description = "Desc1", Name = "Name1", PartitionKey = p1Vendor, ProductId = p1Id, RowKey = $"Product__{p1Id}", VendorCode = p1Vendor };
            TableOperation insertP1 = TableOperation.Insert(_p1);
            _table.Execute(insertP1);
        }

        [TearDown]
        public void PurgeProductData()
        {
            _table.DeleteIfExists();
        }

        [TestCase]
        public void Get_Successful()
        {
            var target = new VendorProductRepository();
            var result = target.Get(_p1.VendorCode, _p1.ProductId).Result;
            Assert.IsNotNull(result);
            Assert.That(result.Description, Is.EqualTo(_p1.Description));
            Assert.That(result.Name, Is.EqualTo(_p1.Name));
            Assert.That(result.Price, Is.EqualTo(_p1.Price));
            Assert.That(result.ProductId, Is.EqualTo(_p1.ProductId));
            Assert.That(result.VendorCode, Is.EqualTo(_p1.VendorCode));
        }

        [TestCase]
        public void Get_ProductIdNotFoundForVendor_NotFound()
        {
            var target = new VendorProductRepository();
            var result = target.Get(_p1.VendorCode, Guid.NewGuid()).Result;
            Assert.IsNull(result);
        }

        [TestCase]
        public void Get_ProductIdForWrongVendorId_NotFound()
        {
            var target = new VendorProductRepository();
            var result = target.Get("WRONG", _p1.ProductId).Result;
            Assert.IsNull(result);
        }

    }
}
