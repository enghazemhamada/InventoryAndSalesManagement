using InventoryAndSalesManagement.Features.Customers;
using InventoryAndSalesManagement.Features.Products;
using InventoryAndSalesManagement.Features.Sales;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndSalesManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalesInvoice>(b =>
            {
                b.Property(si => si.Date).HasDefaultValueSql("GETDATE()");
                b.HasIndex(si => si.InvoiceNo).IsUnique();
            });
        }
    }
}
