using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PIYA_API.Data;

#nullable disable

namespace PIYA_API.Migrations
{
    [DbContext(typeof(PharmacyApiDbContext))]
    partial class PharmacyApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PIYA_API.Model.Coordinates", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("PIYA_API.Model.Pharmacy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CoordinatesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CoordinatesId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Pharmacies");
                });

            modelBuilder.Entity("PIYA_API.Model.PharmacyCompany", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PharmacyCompanies");
                });

            modelBuilder.Entity("PIYA_API.Model.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("PIYA_API.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PharmacyCompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PharmacyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SigningKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TokensInfoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PharmacyCompanyId");

                    b.HasIndex("PharmacyId");

                    b.HasIndex("TokensInfoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PIYA_API.Model.Pharmacy", b =>
                {
                    b.HasOne("PIYA_API.Model.PharmacyCompany", "Company")
                        .WithMany("Pharmacies")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PIYA_API.Model.Coordinates", "Coordinates")
                        .WithMany()
                        .HasForeignKey("CoordinatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PIYA_API.Model.User", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Company");

                    b.Navigation("Coordinates");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("PIYA_API.Model.User", b =>
                {
                    b.HasOne("PIYA_API.Model.PharmacyCompany", null)
                        .WithMany("Staff")
                        .HasForeignKey("PharmacyCompanyId");

                    b.HasOne("PIYA_API.Model.Pharmacy", null)
                        .WithMany("Staff")
                        .HasForeignKey("PharmacyId");

                    b.HasOne("PIYA_API.Model.Token", "TokensInfo")
                        .WithMany()
                        .HasForeignKey("TokensInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TokensInfo");
                });

            modelBuilder.Entity("PIYA_API.Model.Pharmacy", b =>
                {
                    b.Navigation("Staff");
                });

            modelBuilder.Entity("PIYA_API.Model.PharmacyCompany", b =>
                {
                    b.Navigation("Pharmacies");

                    b.Navigation("Staff");
                });
#pragma warning restore 612, 618
        }
    }
}
