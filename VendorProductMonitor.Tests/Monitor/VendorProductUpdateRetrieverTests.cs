using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using VendorProductMonitor.Monitor;

namespace VendorProductMonitor.Tests.Monitor
{
    public class VendorProductUpdateRetrieverTests
    {
        private CloudQueue _queue;

        [SetUp]
        public void Setup()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(ConfigurationManager.AppSettings["ProductUpdateQueueName"]);

            _queue.CreateIfNotExists();            
        }

        [TearDown]
        public void Purge()
        {
            _queue.DeleteIfExists();
        }

        [TestCase]
        public void CheckForProductUpdates_MultipleMessagesInQueue_ReturnsBothUpdates()
        {
            var msg1 = new VendorProductUpdateQueueMessage
            {
                Updates = new List<VendorProductUpdate>
                {
                    new VendorProductUpdate { ProductId = Guid.NewGuid(), VendorCode = "V2" }
                }
            };
            var msg2 = new VendorProductUpdateQueueMessage
            {
                Updates = new List<VendorProductUpdate>
                {
                    new VendorProductUpdate { ProductId = Guid.NewGuid(), VendorCode = "V1" },
                }
            };

            _queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(msg1)));
            _queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(msg2)));

            var target = new VendorProductUpdateRetriever();
            var msgs = target.CheckForProductUpdates().Result.ToList();
            Assert.That(msgs.Count(), Is.EqualTo(2));
        }

        [TestCase]
        public void CheckForProductUpdates_SingleMessageInQueueWithMultipleUpdates_ReturnsBothUpdates()
        {
            var msg1 = new VendorProductUpdateQueueMessage
            {
                Updates = new List<VendorProductUpdate>
                {
                    new VendorProductUpdate { ProductId = Guid.NewGuid(), VendorCode = "V2" },
                    new VendorProductUpdate { ProductId = Guid.NewGuid(), VendorCode = "V1" }
                }
            };

            _queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(msg1)));

            var target = new VendorProductUpdateRetriever();
            var msgs = target.CheckForProductUpdates().Result.ToList();
            Assert.That(msgs.Count(), Is.EqualTo(2));
            Assert.That(msgs[0].VendorCode, Is.EqualTo("V2"));
            Assert.That(msgs[1].VendorCode, Is.EqualTo("V1"));            
        }

        [TestCase]
        public void CheckForProductUpdates_SingleMessageInQueue_MessageIsDeletedFromQueue()
        {
            var msg1 = new VendorProductUpdateQueueMessage
            {
                Updates = new List<VendorProductUpdate>
                {
                    new VendorProductUpdate { ProductId = Guid.NewGuid(), VendorCode = "V2" },
                }
            };

            _queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(msg1)));

            var target = new VendorProductUpdateRetriever();
            var msgs = target.CheckForProductUpdates().Result.ToList();
            Assert.That(msgs.Count, Is.EqualTo(1));

            var queueMsg = _queue.GetMessage();
            Assert.IsNull(queueMsg);
        }

        [TestCase]
        public void CheckForProductUpdates_PoisonMessageInQueue_MessageIsStillDeletedFromQueue()
        {
            var msg1 = "This is a poison message";

            _queue.AddMessage(new CloudQueueMessage(msg1));

            var target = new VendorProductUpdateRetriever();
            bool exceptionCaught = false;
            try
            {
                var msgs = target.CheckForProductUpdates().Result.ToList();
                Assert.That(msgs.Count, Is.EqualTo(1));
            }
            catch (Exception ex)
            {
                exceptionCaught = true;
            }

            Assert.That(exceptionCaught, Is.EqualTo(true), "Should have got exception from poison message");

            Task.Delay(2000).Wait();
            // Message should still have been removed
            var queueMsg = _queue.GetMessage();
            Assert.IsNull(queueMsg);
        }

        [TestCase]
        public void CheckForProductUpdates_QueueEmpty_ReturnsNull()
        {            
            var target = new VendorProductUpdateRetriever();
            var msgs = target.CheckForProductUpdates().Result;
            Assert.IsNull(msgs);
        }
    }
}
