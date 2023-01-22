using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WpfApp1
{
    public partial class FurnitureDbContext : DbContext
    {

        public FurnitureDbContext()
         : base("name=FurnitureDbContext")
        {
            Database.SetInitializer<FurnitureDbContext>(new DropCreateDatabaseIfModelChanges<FurnitureDbContext>());
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }


    }
}
