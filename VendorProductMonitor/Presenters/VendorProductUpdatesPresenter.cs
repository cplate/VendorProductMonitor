using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using VendorProductMonitor.Data.Models;
using VendorProductMonitor.Data.Repositories;
using VendorProductMonitor.Monitor;
using VendorProductMonitor.ViewModels;
using VendorProductMonitor.Views;

namespace VendorProductMonitor.Presenters
{
    public class VendorProductUpdatesPresenter
    {
        private readonly IVendorProductUpdatesView _view;
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorProductRepository _vendorProductRepository;
        private readonly IVendorProductUpdateMonitor _monitor;

        public IDictionary<string, Vendor> VendorLookup { get; set; }
        public LinkedList<VendorProductUpdateViewModel> MostRecentUpdates { get; set; }
        
        private int _maxUpdatesToShow;
        public int MaxUpdatesToShow { get { return _maxUpdatesToShow; } set { _maxUpdatesToShow = value; } }

        public VendorProductUpdatesPresenter(IVendorProductUpdatesView view, IVendorRepository vendorRepos, IVendorProductRepository vendorProductRepos, IVendorProductUpdateMonitor monitor)
        {
            _view = view;
            _vendorRepository = vendorRepos;
            _vendorProductRepository = vendorProductRepos;
            _monitor = monitor;

            MostRecentUpdates = new LinkedList<VendorProductUpdateViewModel>();

            if (!int.TryParse(ConfigurationManager.AppSettings["MaxUpdatesToShow"], out _maxUpdatesToShow))
            {
                _maxUpdatesToShow = 50;
            }
        }

        public async Task ShowView()
        {
            _view.Show();
            
            var vendors = (await _vendorRepository.GetAll()).ToList();
            _view.SetVendorList(vendors);
            VendorLookup = vendors.ToDictionary(x => x.Code, x => x);

            _monitor.VendorProductsUpdated += VendorProductsUpdated;
            _monitor.Start();
        }

        private async void VendorProductsUpdated(object sender, VendorProductsUpdatedEventArgs e)
        {
            await HandleVendorProductsUpdated(e.Updates);
        }

        public async Task HandleVendorProductsUpdated(IEnumerable<VendorProductUpdate> updates)
        {
            foreach (var updatedProduct in updates)
            {
                // Need to grab current info about the product
                var productDetail = await _vendorProductRepository.Get(updatedProduct.VendorCode, updatedProduct.ProductId);

                Vendor vendor;
                if (!VendorLookup.TryGetValue(updatedProduct.VendorCode, out vendor))
                {
                    // Should log an error somewhere if this is a realistic scenario
                    continue;
                }

                var viewModel = new VendorProductUpdateViewModel
                {
                    Price = productDetail.Price,
                    ProductName = productDetail.Name,
                    ProductDescription = productDetail.Description,
                    VendorName = vendor.Name
                };

                MostRecentUpdates.AddFirst(viewModel);
                while (MostRecentUpdates.Count > _maxUpdatesToShow)
                {
                    MostRecentUpdates.RemoveLast();
                }                
            }
            
            _view.SetProductUpdateList(MostRecentUpdates);
        }
    }
}