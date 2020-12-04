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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("host=postgres;User Id=admin;Password=password;Database=postgres;Port=5432;");

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Broker> Brokers { get; set; }
        public virtual DbSet<Email> Email { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>().HasKey(m => new { m.Name });
            builder.Entity<Broker>().HasKey(m => new { m.ClientName, m.NPN });
            builder.Entity<Broker>().HasOne<Client>(m => m.Client).WithMany(m => m.Brokers).HasForeignKey(m => new { m.ClientName });

            builder.Entity<Email>().HasAlternateKey(m => new { m.ClientName, m.ParentNPN, m.EmailAddressType });
            builder.Entity<Email>().HasOne<Client>(m => m.Client);
            builder.Entity<Email>().HasOne<Broker>(m => m.Parent).WithMany(m => m.EmailAddresses).HasForeignKey(m => new { m.ClientName, m.ParentNPN });
        }
    }
}