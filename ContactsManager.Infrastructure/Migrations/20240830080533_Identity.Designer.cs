﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactsManager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240830080533_Identity")]
    partial class Identity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ContactsManager.Core.Domain.IdentityEntities.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("ContactsManager.Core.Domain.IdentityEntities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            CountryName = "Philippines"
                        },
                        new
                        {
                            CountryId = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            CountryName = "Thailand"
                        },
                        new
                        {
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            CountryName = "China"
                        },
                        new
                        {
                            CountryId = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            CountryName = "Palestinian Territory"
                        },
                        new
                        {
                            CountryId = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            CountryName = "China"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceiveLetters")
                        .HasColumnType("bit");

                    b.Property<string>("TIN")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("AB1354323")
                        .HasColumnName("TaxIdentificationNumber");

                    b.HasKey("PersonId");

                    b.HasIndex("CountryId");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"),
                            Address = "4 Parkside Point",
                            CountryId = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            DateOfBirth = new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mwebsdale0@people.com.cn",
                            Gender = "Female",
                            PersonName = "Marguerite",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"),
                            Address = "6 Morningstar Circle",
                            CountryId = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            DateOfBirth = new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ushears1@globo.com",
                            Gender = "Female",
                            PersonName = "Ursa",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"),
                            Address = "73 Heath Avenue",
                            CountryId = new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"),
                            DateOfBirth = new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "fbowsher2@howstuffworks.com",
                            Gender = "Male",
                            PersonName = "Franchot",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"),
                            Address = "83187 Merry Drive",
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "asarvar3@dropbox.com",
                            Gender = "Male",
                            PersonName = "Angie",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"),
                            Address = "50467 Holy Cross Crossing",
                            CountryId = new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"),
                            DateOfBirth = new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ttregona4@stumbleupon.com",
                            Gender = "Gender",
                            PersonName = "Tani",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"),
                            Address = "97570 Raven Circle",
                            CountryId = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            DateOfBirth = new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mlingfoot5@netvibes.com",
                            Gender = "Male",
                            PersonName = "Mitchael",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("29339209-63f5-492f-8459-754943c74abf"),
                            Address = "57449 Brown Way",
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mjarrell6@wisc.edu",
                            Gender = "Male",
                            PersonName = "Maddy",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"),
                            Address = "4 Stuart Drive",
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "pretchford7@virginia.edu",
                            Gender = "Female",
                            PersonName = "Pegeen",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("012107df-862f-4f16-ba94-e5c16886f005"),
                            Address = "413 Sachtjen Way",
                            CountryId = new Guid("12e15727-d369-49a9-8b13-bc22e9362179"),
                            DateOfBirth = new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "hmosco8@tripod.com",
                            Gender = "Male",
                            PersonName = "Hansiain",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"),
                            Address = "484 Clarendon Court",
                            CountryId = new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"),
                            DateOfBirth = new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "lwoodwing9@wix.com",
                            Gender = "Male",
                            PersonName = "Lombard",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"),
                            Address = "2 Warrior Avenue",
                            CountryId = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            DateOfBirth = new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mconachya@va.gov",
                            Gender = "Female",
                            PersonName = "Minta",
                            ReceiveLetters = false
                        },
                        new
                        {
                            PersonId = new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"),
                            Address = "9334 Fremont Street",
                            CountryId = new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"),
                            DateOfBirth = new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "vklussb@nationalgeographic.com",
                            Gender = "Female",
                            PersonName = "Verene",
                            ReceiveLetters = false
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.HasOne("Entities.Country", "Country")
                        .WithMany("Persons")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("ContactsManager.Core.Domain.IdentityEntities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
