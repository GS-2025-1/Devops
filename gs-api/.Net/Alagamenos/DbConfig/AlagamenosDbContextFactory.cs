using Alagamenos.DbConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Alagamenos.DbConfig
{
    public class AlagamenosDbContextFactory : IDesignTimeDbContextFactory<AlagamenosDbContext>
    {
        public AlagamenosDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connectionString = config.GetConnectionString("FiapOracleDb");

            var optionsBuilder = new DbContextOptionsBuilder<AlagamenosDbContext>();
            optionsBuilder.UseOracle(connectionString); 

            return new AlagamenosDbContext(optionsBuilder.Options);
        }
    }
}