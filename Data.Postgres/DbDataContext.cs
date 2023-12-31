﻿using Microsoft.EntityFrameworkCore;

namespace Data.Postgres
{
    public class DataContext : DbContext
    {
        public DbSet<DbCustomer> customers { get; set; }
        public DbSet<DbEmailTemplate> email_templates { get; set; }

        public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}
