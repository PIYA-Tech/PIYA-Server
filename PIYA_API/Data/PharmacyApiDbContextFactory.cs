using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PIYA_API.Data;

/// <summary>
/// Design-time factory for PharmacyApiDbContext
/// Used by EF Core tools (migrations, database update) at design time
/// </summary>
public class PharmacyApiDbContextFactory : IDesignTimeDbContextFactory<PharmacyApiDbContext>
{
    public PharmacyApiDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.Production.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in appsettings.Production.json");
        }

        // Build DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<PharmacyApiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new PharmacyApiDbContext(optionsBuilder.Options);
    }
}
