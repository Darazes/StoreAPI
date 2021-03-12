using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class StoreContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Procurement> Procurements { get; set; }

        public DbSet<Product_request> Product_requests { get; set; }

        public DbSet<Product_shipment> Product_shipment { get; set; }

        public DbSet<Product_storage> Product_storage { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Storage> Storages { get; set; }

        public DbSet<Type> Types { get; set; }

        public StoreContext() : base("StoreContext")
        { }

    } 
}