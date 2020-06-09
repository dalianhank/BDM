using System.IO;
using BDM.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BDM.Data.Concrete
{
    public class BDMEntitiesDB : DbContext
    {
        public BDMEntitiesDB() : base() { }

        public BDMEntitiesDB(DbContextOptions<BDMEntitiesDB> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Server=localhost;User Id=tdadmin;Password=password;Database=BDMPGDatabase;Port=3306;");

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Broker> Brokers { get; set; }
        public virtual DbSet<Email> Email { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>().HasKey(m => new { m.Name });
            builder.Entity<Broker>().HasKey(m => new { m.ClientName, m.NPN });
            builder.Entity<Broker>().HasOne<Client>(m => m.Client).WithMany(m => m.Brokers).HasForeignKey(m => new { m.ClientName });
        }
    }
}