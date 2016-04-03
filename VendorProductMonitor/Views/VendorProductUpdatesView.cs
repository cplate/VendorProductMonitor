using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VendorProductMonitor.Data.Models;
using VendorProductMonitor.ViewModels;

namespace VendorProductMonitor.Views
{
    public partial class VendorProductUpdatesView : Form, IVendorProductUpdatesView
    {               
        public VendorProductUpdatesView()
        {
            InitializeComponent();
            
            vendorGrid.Visible = false;
            vendorsLoadingLabel.Visible = true;
            productUpdatesGrid.Visible = false;
            waitingForUpdatesLabel.Visible = true;
                             
            FormClosing += VendorProductUpdatesView_FormClosing;
            FormClosed += VendorProductUpdatesView_FormClosed;
        }

        public event EventHandler<EventArgs> StopMonitoring;

        private void VendorProductUpdatesView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void VendorProductUpdatesView_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMonitoring?.Invoke(this, new EventArgs());
        }

        public void SetProductUpdateList(IEnumerable<VendorProductUpdateViewModel> updatedProducts)
        {
            Invoke((Action)(() =>
            {
                productUpdatesGrid.DataSource = updatedProducts.ToList();
                waitingForUpdatesLabel.Visible = false;
                productUpdatesGrid.Visible = true;
            }));
        }

        public void SetVendorList(IEnumerable<Vendor> vendors)
        {
            Invoke((Action)(() => 
            {
                vendorGrid.DataSource = vendors;
                vendorsLoadingLabel.Visible = false;
                vendorGrid.Visible = true;                
            }));
        }
    }
}
