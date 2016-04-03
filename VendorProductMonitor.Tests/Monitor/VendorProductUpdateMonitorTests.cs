using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProductMonitor.Monitor;

namespace VendorProductMonitor.Tests.Monitor
{
    public class VendorProductUpdateMonitorTests
    {
        [TestCase]
        public void Start_StartsPolling()
        {
            var numUpdates = 0;

            var dataRetrieverMock = new Mock<IVendorProductUpdateRetriever>();
            dataRetrieverMock.SetupSequence(x => x.CheckForProductUpdates())
                .Returns(Task.FromResult<IEnumerable<VendorProductUpdate>>(null))
                .Returns(Task.FromResult(new List<VendorProductUpdate> { new VendorProductUpdate() }.AsEnumerable())); 
                       
            var target = new VendorProductUpdateMonitor(dataRetrieverMock.Object);
            target.VendorProductsUpdated += delegate { numUpdates++; };

            target.Start();
            Task.Delay(1500).Wait();
            target.Stop();

            Assert.That(numUpdates, Is.EqualTo(1));
            dataRetrieverMock.Verify(x => x.CheckForProductUpdates(), Times.AtMost(2));
        }

        [TestCase]
        public void Stop_StopsPolling()
        {
            var dataRetrieverMock = new Mock<IVendorProductUpdateRetriever>();
            dataRetrieverMock.Setup(x => x.CheckForProductUpdates()).Returns(Task.FromResult<IEnumerable<VendorProductUpdate>>(null));

            var target = new VendorProductUpdateMonitor(dataRetrieverMock.Object);

            target.Start();
            Task.Delay(1500).Wait();
            target.Stop();
            Task.Delay(2500).Wait(); // wait a bit longer to ensure we didnt make any more poll requests

            dataRetrieverMock.Verify(x => x.CheckForProductUpdates(), Times.AtMost(2));
        }
    }
}
