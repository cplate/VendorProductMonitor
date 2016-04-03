using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Models;
using VendorProductMonitor.Data.Repositories;
using VendorProductMonitor.Monitor;
using VendorProductMonitor.Presenters;
using VendorProductMonitor.ViewModels;
using VendorProductMonitor.Views;

namespace VendorProductMonitor.Tests.Presenters
{
    public class VendorProductUpdatesPresenterTests
    {
        [TestCase]
        public void ShowView_RetrievesVendors()
        {
            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            vendorReposMock.Setup(x => x.GetAll()).Returns(Task.FromResult(new List<Vendor> {
                new Vendor() { Code = "V1" },
                new Vendor() { Code = "V2" },
                new Vendor() { Code = "V3" },
            }.AsEnumerable()));
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object, vendorProductReposMock.Object, monitorMock.Object);
            target.ShowView().Wait();

            vendorReposMock.Verify(x => x.GetAll(), Times.Once);
            Assert.That(target.VendorLookup.Count, Is.EqualTo(3));
        }

        [TestCase]
        public void ShowView_StartsMonitor()
        {
            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object, vendorProductReposMock.Object, monitorMock.Object);
            target.ShowView().Wait();

            monitorMock.Verify(x => x.Start(), Times.Once);
        }

        [TestCase]
        public void VendorProductsUpdated_SingleUpdate_ViewUpdatedOnce()
        {
            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            vendorProductReposMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(new VendorProduct()));
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object,
                vendorProductReposMock.Object, monitorMock.Object)
            {
                VendorLookup = new Dictionary<string, Vendor> {{"V1", new Vendor {Name = "Vendor1"}}}
            };

            target.HandleVendorProductsUpdated(new List<VendorProductUpdate> { new VendorProductUpdate { VendorCode = "V1", ProductId = Guid.NewGuid() } }).Wait();

            viewMock.Verify(x => x.SetProductUpdateList(It.IsAny<IEnumerable<VendorProductUpdateViewModel>>()), Times.Once);
            vendorProductReposMock.VerifyAll();
            Assert.That(target.MostRecentUpdates.Count, Is.EqualTo(1));
        }

        public void VendorProductsUpdated_ViewModelPopulatedProperly()
        {
            var vendorId = "V1";
            var productId = Guid.NewGuid();
            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            vendorProductReposMock.Setup(x => x.Get("V1", productId)).Returns(Task.FromResult(new VendorProduct { VendorCode = vendorId, Name = "Prod1", Price = 3.99, Description = "Desc" }));
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object,
                vendorProductReposMock.Object, monitorMock.Object)
            {
                VendorLookup = new Dictionary<string, Vendor> {{vendorId, new Vendor {Name = "Vendor1"}}}
            };

            target.HandleVendorProductsUpdated(new List<VendorProductUpdate> { new VendorProductUpdate { VendorCode = vendorId, ProductId = productId } }).Wait();

            Assert.That(target.MostRecentUpdates.Count, Is.EqualTo(1));
            var vm = target.MostRecentUpdates.First.Value;
            Assert.That(vm.VendorName, Is.EqualTo("Vendor1"));
            Assert.That(vm.ProductName, Is.EqualTo("Prod1"));
            Assert.That(vm.Price, Is.EqualTo(3.99));
            Assert.That(vm.ProductDescription, Is.EqualTo("Desc"));
        }

        [TestCase]
        public void VendorProductsUpdated_MultipleUpdates_ViewUpdatedOnce()
        {
            var vendorId1 = "V1";
            var productId1 = Guid.NewGuid();
            var vendorId2 = "V2";
            var productId2 = Guid.NewGuid();

            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            vendorProductReposMock.Setup(x => x.Get(vendorId1, productId1)).Returns(Task.FromResult(new VendorProduct()));
            vendorProductReposMock.Setup(x => x.Get(vendorId2, productId2)).Returns(Task.FromResult(new VendorProduct()));
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object,
                vendorProductReposMock.Object, monitorMock.Object)
            {
                VendorLookup =
                    new Dictionary<string, Vendor>
                    {
                        {vendorId1, new Vendor {Name = "Vendor1"}},
                        {vendorId2, new Vendor {Name = "Vendor2"}}
                    }
            };

            target.HandleVendorProductsUpdated(new List<VendorProductUpdate> {
                new VendorProductUpdate { VendorCode = vendorId1, ProductId = productId1 },
                new VendorProductUpdate { VendorCode = vendorId2, ProductId = productId2 } }).Wait();

            vendorProductReposMock.VerifyAll();
            viewMock.Verify(x => x.SetProductUpdateList(It.IsAny<IEnumerable<VendorProductUpdateViewModel>>()), Times.Once);
            Assert.That(target.MostRecentUpdates.Count, Is.EqualTo(2));
        }

        [TestCase]
        public void HandleVendorProductsUpdated_UpdateForMissingVendors_UpdateNotAddedToList()
        {
            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            vendorProductReposMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(new VendorProduct { VendorCode = "V1" }));
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object,
                vendorProductReposMock.Object, monitorMock.Object) {VendorLookup = new Dictionary<string, Vendor>()};

            target.HandleVendorProductsUpdated(new List<VendorProductUpdate> { new VendorProductUpdate { VendorCode = "V2", ProductId = Guid.NewGuid() } }).Wait();

            viewMock.Verify(x => x.SetProductUpdateList(It.IsAny<IEnumerable<VendorProductUpdateViewModel>>()), Times.Once);
            Assert.That(target.MostRecentUpdates.Count, Is.EqualTo(0));
        }

        public void VendorProductsUpdated_OldUpdatesDiscardedWhenFull()
        {
            var vendorId1 = "V1";
            var productId1 = Guid.NewGuid();
            var vendorId2 = "V2";
            var productId2 = Guid.NewGuid();

            var viewMock = new Mock<IVendorProductUpdatesView>();
            var vendorReposMock = new Mock<IVendorRepository>();
            var vendorProductReposMock = new Mock<IVendorProductRepository>();
            vendorProductReposMock.Setup(x => x.Get(vendorId1, productId1)).Returns(Task.FromResult(new VendorProduct()));
            vendorProductReposMock.Setup(x => x.Get(vendorId2, productId2)).Returns(Task.FromResult(new VendorProduct()));
            var monitorMock = new Mock<IVendorProductUpdateMonitor>();

            var target = new VendorProductUpdatesPresenter(viewMock.Object, vendorReposMock.Object,
                vendorProductReposMock.Object, monitorMock.Object)
            {
                VendorLookup =
                    new Dictionary<string, Vendor>
                    {
                        {vendorId1, new Vendor {Name = "Vendor1"}},
                        {vendorId2, new Vendor {Name = "Vendor2"}}
                    },
                MaxUpdatesToShow = 3
            };
            target.MostRecentUpdates.AddFirst(new VendorProductUpdateViewModel());
            target.MostRecentUpdates.AddFirst(new VendorProductUpdateViewModel());

            target.HandleVendorProductsUpdated(new List<VendorProductUpdate> {
                new VendorProductUpdate { VendorCode = vendorId1, ProductId = productId1 },
                new VendorProductUpdate { VendorCode = vendorId2, ProductId = productId2 } }).Wait();

            viewMock.Verify(x => x.SetProductUpdateList(It.IsAny<IEnumerable<VendorProductUpdateViewModel>>()), Times.Once);
            Assert.That(target.MostRecentUpdates.Count, Is.EqualTo(3));
            Assert.That(target.MostRecentUpdates.First.Value.VendorName, Is.EqualTo("Vendor1"));
            Assert.That(target.MostRecentUpdates.First.Next?.Value.VendorName, Is.EqualTo("Vendor2"));
            Assert.That(target.MostRecentUpdates.First.Next?.Next?.Value.VendorName, Is.EqualTo(null));
        }
    }
}
