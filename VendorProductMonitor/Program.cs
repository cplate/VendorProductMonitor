using Ninject;
using System;
using Ninject.Extensions.Conventions;
using System.Windows.Forms;
using VendorProductMonitor.Presenters;

namespace VendorProductMonitor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Using configuration by convention for simplicity here
            var kernel = new StandardKernel();
            kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());

            var p = kernel.Get<VendorProductUpdatesPresenter>();

            p.ShowView(); // Not awaiting this intentionally
            Application.Run();
        }
    }
}
