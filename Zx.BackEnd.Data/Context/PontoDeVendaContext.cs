using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Zx.BackEnd.Data.Entity;

namespace Zx.BackEnd.Data.Context
{
    public enum EnumProvider
    {
        Unknown = 9999,
        MySQL = 1,
        SQLite = 2,
        SQLServer = 3,
        PostgreSQL = 4,
        InMemory = 5
    }

    public class PontoDeVendaContext : DbContext
    {

        public EnumProvider Provider { get; private set; }
        public string ConnectionString { get; private set; }

        public DbSet<PontoDeVenda> PontoDeVenda { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<CoverageArea> CoverageArea { get; set; }

        public PontoDeVendaContext() { }

        public PontoDeVendaContext(DbContextOptions<PontoDeVendaContext> options, EnumProvider provider, string connectionString) : base(GetOptions(provider, connectionString, options)) { Provider = provider; ConnectionString = connectionString; EnsureCreated(); }
        public PontoDeVendaContext(EnumProvider provider, string connectionString = null) : base(GetOptions(provider, connectionString)) { Provider = provider; ConnectionString = connectionString; EnsureCreated(); }
        public PontoDeVendaContext(DbContextOptions<PontoDeVendaContext> options) : base(GetOptions(EnumProvider.SQLite, "", options)) { Provider = EnumProvider.SQLite; ConnectionString = ""; EnsureCreated(); }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<PontoDeVenda>(new PontoDeVendaConfiguration());
            modelBuilder.Entity<PontoDeVenda>().ToTable("PontoDeVenda");
            base.OnModelCreating(modelBuilder);
        }


        private void EnsureCreated()
        {
            try
            {
                this.Database.EnsureCreated();
            }
            catch 
            {
                // ignored
            }
        }

        private static DbContextOptions<PontoDeVendaContext> GetOptions(EnumProvider provider, string connectionString = null, DbContextOptions<PontoDeVendaContext> dbContextOptions = null)
        {
            if (string.IsNullOrEmpty(connectionString) && dbContextOptions != null)
                return dbContextOptions;
            else
            {
                if (string.IsNullOrEmpty(connectionString) && provider == EnumProvider.SQLite)
                    connectionString = $"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SQLite.db")}";

                if (string.IsNullOrEmpty(connectionString) && provider == EnumProvider.InMemory)
                    connectionString = "InMemoryDataBase";

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException(nameof(connectionString), "Não existe uma conexão.");

                DbContextOptions<PontoDeVendaContext> _dbContextOptions;
                switch (provider)
                {
                    case EnumProvider.MySQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseMySql<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseMySql<PontoDeVendaContext>(connectionString).Options;
                        break;
                    case EnumProvider.SQLServer:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseSqlServer<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseSqlServer<PontoDeVendaContext>(connectionString).Options;

                        break;
                    case EnumProvider.SQLite:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseSqlite<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseSqlite<PontoDeVendaContext>(connectionString).Options;
                        break;
                    case EnumProvider.PostgreSQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseNpgsql<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseNpgsql<PontoDeVendaContext>(connectionString).Options;
                        break;
                    case EnumProvider.InMemory:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseInMemoryDatabase<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseInMemoryDatabase<PontoDeVendaContext>(connectionString).Options;
                        break;
                    default:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<PontoDeVendaContext>(dbContextOptions).UseSqlite<PontoDeVendaContext>(connectionString).Options : new DbContextOptionsBuilder<PontoDeVendaContext>().UseSqlite<PontoDeVendaContext>(connectionString).Options;
                        break;
                }
                return _dbContextOptions;
            }
        }
    }
}
