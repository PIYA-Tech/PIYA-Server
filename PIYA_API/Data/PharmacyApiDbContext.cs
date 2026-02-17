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
        public DbSet<InventoryBatch> InventoryBatches { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<DoctorNote> DoctorNotes { get; set; }
        public DbSet<QRToken> QRTokens { get; set; }
        
        // Pharmacy Staff & Permissions
        public DbSet<PharmacyStaff> PharmacyStaff { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

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
                .HasOne(dp => dp.User)
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
            
            // PharmacyStaff - Pharmacy relationship
            modelBuilder.Entity<PharmacyStaff>()
                .HasOne(ps => ps.Pharmacy)
                .WithMany()
                .HasForeignKey(ps => ps.PharmacyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // PharmacyStaff - User relationship
            modelBuilder.Entity<PharmacyStaff>()
                .HasOne(ps => ps.User)
                .WithMany()
                .HasForeignKey(ps => ps.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserPermission - User relationship
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserPermission - GrantedBy relationship
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.GrantedBy)
                .WithMany()
                .HasForeignKey(up => up.GrantedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Indexes for PharmacyStaff
            modelBuilder.Entity<PharmacyStaff>()
                .HasIndex(ps => ps.PharmacyId);
            
            modelBuilder.Entity<PharmacyStaff>()
                .HasIndex(ps => ps.UserId);
            
            modelBuilder.Entity<PharmacyStaff>()
                .HasIndex(ps => new { ps.PharmacyId, ps.UserId });
            
            // Indexes for UserPermission
            modelBuilder.Entity<UserPermission>()
                .HasIndex(up => up.UserId);
            
            modelBuilder.Entity<UserPermission>()
                .HasIndex(up => up.Permission);
            
            modelBuilder.Entity<UserPermission>()
                .HasIndex(up => new { up.UserId, up.Permission });
        }
    }
}
