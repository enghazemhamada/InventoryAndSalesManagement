namespace InventoryAndSalesManagement.Features.Reports
{
    public class ReportViewModel
    {
        public decimal TotalSales { get; set; }
        public int NumberOfSales { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
