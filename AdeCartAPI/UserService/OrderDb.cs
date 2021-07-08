using AdeCartAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.UserService
{
    public class OrderDb :DbContext
    {
        public OrderDb(DbContextOptions<OrderDb> options):base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCart> Carts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
