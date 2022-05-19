using eruditionis.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eruditionis.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TestingEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        public DbSet<TestingEntity> TestingEntities { get; set; }

    }
    
}
