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
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<MedicalDocument> MedicalDocuments { get; set; }
        public DbSet<DeviceToken> DeviceTokens { get; set; }
        
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
            
            // EmailVerificationToken - User relationship
            modelBuilder.Entity<EmailVerificationToken>()
                .HasOne(evt => evt.User)
                .WithMany()
                .HasForeignKey(evt => evt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<EmailVerificationToken>()
                .HasIndex(evt => evt.TokenHash);
            
            modelBuilder.Entity<EmailVerificationToken>()
                .HasIndex(evt => evt.Email);
            
            // PasswordResetToken - User relationship
            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(prt => prt.User)
                .WithMany()
                .HasForeignKey(prt => prt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<PasswordResetToken>()
                .HasIndex(prt => prt.TokenHash);
            
            modelBuilder.Entity<PasswordResetToken>()
                .HasIndex(prt => prt.Email);
            
            // MedicalDocument - User relationship
            modelBuilder.Entity<MedicalDocument>()
                .HasOne(md => md.User)
                .WithMany()
                .HasForeignKey(md => md.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasOne(md => md.UploadedBy)
                .WithMany()
                .HasForeignKey(md => md.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasOne(md => md.VerifiedBy)
                .WithMany()
                .HasForeignKey(md => md.VerifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasOne(md => md.Appointment)
                .WithMany()
                .HasForeignKey(md => md.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasOne(md => md.Prescription)
                .WithMany()
                .HasForeignKey(md => md.PrescriptionId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasIndex(md => md.UserId);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasIndex(md => md.DocumentType);
            
            modelBuilder.Entity<MedicalDocument>()
                .HasIndex(md => md.FileHash);
            
            // DeviceToken - User relationship
            modelBuilder.Entity<DeviceToken>()
                .HasOne(dt => dt.User)
                .WithMany()
                .HasForeignKey(dt => dt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DeviceToken>()
                .HasIndex(dt => dt.Token)
                .IsUnique();
            
            modelBuilder.Entity<DeviceToken>()
                .HasIndex(dt => dt.UserId);
        }
    }
}
