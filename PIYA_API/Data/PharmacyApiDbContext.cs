using Microsoft.EntityFrameworkCore;
using PIYA_API.Model;

namespace PIYA_API.Data
{
    public class PharmacyApiDbContext(DbContextOptions<PharmacyApiDbContext> options) : DbContext(options)
    {
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyCompany> PharmacyCompanies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
