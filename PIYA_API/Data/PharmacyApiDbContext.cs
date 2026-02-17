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
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<TwoFactorAuth> TwoFactorAuths { get; set; }
        
        // Healthcare entities
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PharmacyInventory> PharmacyInventories { get; set; }
        public DbSet<DoctorNote> DoctorNotes { get; set; }
        public DbSet<QRToken> QRTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - TwoFactorAuth (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.TwoFactorAuth)
                .WithOne(t => t.User)
                .HasForeignKey<TwoFactorAuth>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - DoctorProfile (One-to-One)
            modelBuilder.Entity<DoctorProfile>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<DoctorProfile>(dp => dp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AuditLog - User (Many-to-One, nullable)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.CreatedAt);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.UserId);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.Action);
        }
    }
}
