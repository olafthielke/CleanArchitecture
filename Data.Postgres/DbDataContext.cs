using Microsoft.EntityFrameworkCore;

namespace Data.Postgres
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<DbCustomer> customers { get; set; }
        public DbSet<DbEmailTemplate> email_templates { get; set; }
        public DbSet<DbSmsTemplate> sms_templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}
