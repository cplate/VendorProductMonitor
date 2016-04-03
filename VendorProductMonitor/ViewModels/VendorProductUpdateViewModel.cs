namespace VendorProductMonitor.ViewModels
{
    // Bundle together info from the vendor and the product for our display        
    public class VendorProductUpdateViewModel
    {        
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
    }
}
