using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    using System.Data.Entity;

    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Restock> Restocks { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public System.Data.Entity.DbSet<FinalProject.Models.User> Users { get; set; }
    }

}