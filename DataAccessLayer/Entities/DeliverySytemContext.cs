using System;
using System.Collections.Generic;
using DataAccessLayer.DependencyInjections.Extensions;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Entities;

public partial class DeliverySytemContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DeliverySytemContext(DbContextOptions<DeliverySytemContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicePriceRule> PriceRules { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreStaff> StoreStaffs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    public virtual DbSet<Warehouse> Warehouses {  get; set; }

    public virtual DbSet<WarehouseSlot> WarehouseSlots { get; set; }

    public virtual DbSet<SlotReservation> SlotReservations {  get; set; }

    public virtual DbSet<KycDocument> KycDocuments { get; set; }    

    public virtual DbSet<KycSubmission> KycSubmissions {  get; set; }
    public virtual DbSet<ServiceAddon> ServiceAddons { get; set; }
    public virtual DbSet<Contract> Contracts { get; set; }
    
    public virtual DbSet<OrderWarehouseSlot> OrderWarehouseSlots { get; set; }
    public virtual DbSet<ServiceSizeOption> ServiceSizeOptions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__091C2AFB08EA6C9D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.AddressLine).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(80);
            entity.Property(e => e.District).HasMaxLength(80);
            entity.Property(e => e.Label).HasMaxLength(50);
            entity.Property(e => e.Ward).HasMaxLength(80);

            entity.HasOne(d => d.Store).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Addresses_Stores");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Addresses_Users");
        });

        modelBuilder.Entity<Category>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(200);
            b.Property(x => x.Path).HasMaxLength(1000);
            b.Property(x => x.Icon).HasMaxLength(200);
            b.Property(x => x.ThumbnailUrl).HasMaxLength(500);

            b.HasOne(x => x.Parent)
             .WithMany(x => x.InverseParent)
             .HasForeignKey(x => x.ParentId)
             .OnDelete(DeleteBehavior.Restrict); // tránh cascade vòng

            // Index tối ưu
            b.HasIndex(x => x.ParentId);
            b.HasIndex(x => new { x.ParentId, x.SortOrder });
            b.HasIndex(x => x.Path);


            b.HasIndex(x => new { x.StoreId, x.Slug }).IsUnique();

            // Check: ParentId != Id
            b.ToTable(t => t.HasCheckConstraint("CK_Category_Parent_Not_Self", "[ParentId] IS NULL OR [ParentId] <> [Id]"));

        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__A4AE64D8B858128B");
            entity.Property(e => e.Id).ValueGeneratedNever(); // Không tự động sinh, sử dụng giá trị từ User.Id
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.KycLevel)
                .HasMaxLength(20)
                .HasDefaultValue("None");
            entity.Property(e => e.PreferredLang).HasMaxLength(10);
            entity.Property(e => e.Tier)
                .HasMaxLength(20)
                .HasDefaultValue("Basic");

            // Sửa: Xác định rõ mối quan hệ 1-1 với shared primary key
            entity.HasOne(d => d.User)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Id) // Sử dụng Id làm khóa ngoại
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Customers_Users");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__6A4BEDD68F15C3E7");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.FromUser).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Users");
            
            entity.HasOne(d => d.Order)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderId)
                .IsRequired(false)                       // ← bắt buộc thêm
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Orders");

            entity.HasOne(d => d.ToStore).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ToStoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Stores");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__C3905BCF19CCE46F");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DistanceKm).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue(StatusValue.Draft);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.DropoffAddress).WithMany(p => p.OrderDropoffAddresses)
                .HasForeignKey(d => d.DropoffAddressId)
                .HasConstraintName("FK_Orders_Dropoff");

            entity.HasOne(d => d.PickupAddress).WithMany(p => p.OrderPickupAddresses)
                .HasForeignKey(d => d.PickupAddressId)
                .HasConstraintName("FK_Orders_Pickup");

            entity.HasOne(d => d.Quotation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK_Orders_Quotations");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Stores");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ItemName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Description)
                  .HasMaxLength(500);

            entity.Property(e => e.LengthM).HasPrecision(5, 2);
            entity.Property(e => e.WidthM).HasPrecision(5, 2);
            entity.Property(e => e.HeightM).HasPrecision(5, 2);
            entity.Property(e => e.WeightKg).HasPrecision(6, 2);

            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);

            // Quan hệ với Order (1-N)
            entity.HasOne(d => d.Order)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(d => d.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với Service (1-N, có thể null)
            entity.HasOne(d => d.Service)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(d => d.ServiceId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__9B556A3861C7126E");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Method).HasMaxLength(30);
            entity.Property(e => e.Provider).HasMaxLength(60);
            entity.Property(e => e.ProviderTxnId).HasMaxLength(120);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Pending);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__E1975293941AD414");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Sent);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotations_Customers");

            entity.HasOne(d => d.Store).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.StoreId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotations_Stores");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__C51BB00A8BC5FA65");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Unit)
                .HasMaxLength(30)
                .HasDefaultValue("Job");

            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Services_Categories");

            entity.HasOne(d => d.Store).WithMany(p => p.Services)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Stores");
            entity.HasMany(x => x.PriceRules)
                .WithOne(r => r.Service)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Addons)
                .WithOne(a => a.Service)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.SizeOptions)
                .WithOne(s => s.Service)
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);


        });

        modelBuilder.Entity<ServicePriceRule>(b =>
        {
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.Property(x => x.MinVolumeM3).HasColumnType("decimal(18,4)");
            b.Property(x => x.MaxVolumeM3).HasColumnType("decimal(18,4)");
            b.Property(x => x.MinAreaM2).HasColumnType("decimal(18,4)");
            b.Property(x => x.MaxAreaM2).HasColumnType("decimal(18,4)");
            b.Property(x => x.MinQty).HasColumnType("decimal(18,2)");
            b.Property(x => x.MaxQty).HasColumnType("decimal(18,2)");

            // Index giúp tìm rule hiệu quả
            b.HasIndex(x => new { x.ServiceId, x.ValidFrom, x.ValidTo });
            b.HasIndex(x => new { x.ServiceId, x.MinVolumeM3, x.MaxVolumeM3, x.MinDays, x.MaxDays });

            // Check cơ bản để tránh range sai
            b.ToTable(t => t.HasCheckConstraint("CK_ServicePriceRule_Ranges", @"
            (MinVolumeM3 IS NULL OR MaxVolumeM3 IS NULL OR MinVolumeM3 <= MaxVolumeM3) AND
            (MinAreaM2  IS NULL OR MaxAreaM2  IS NULL OR MinAreaM2  <= MaxAreaM2)  AND
            (MinQty     IS NULL OR MaxQty     IS NULL OR MinQty     <= MaxQty)     AND
            (MinDays    IS NULL OR MaxDays    IS NULL OR MinDays    <= MaxDays)
        "));
        });

        modelBuilder.Entity<ServiceAddon>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200);
            b.Property(x => x.Value).HasColumnType("decimal(18,2)");
            b.HasIndex(x => new { x.ServiceId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<ServiceSizeOption>(b =>
        {
            b.Property(x => x.Code).HasMaxLength(50);
            b.Property(x => x.DisplayName).HasMaxLength(200);
            b.Property(x => x.VolumeM3).HasColumnType("decimal(18,4)");
            b.Property(x => x.AreaM2).HasColumnType("decimal(18,4)");
            b.Property(x => x.MaxWeightKg).HasColumnType("decimal(18,2)");
            b.Property(x => x.PriceOverride).HasColumnType("decimal(18,2)");

            b.HasIndex(x => new { x.ServiceId, x.Code }).IsUnique();
        });
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3B82F101588E2030");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.KycLevel)
                .HasMaxLength(20)
                .HasDefaultValue("None");
            entity.Property(e => e.LegalName).HasMaxLength(150);
            entity.Property(e => e.LicenseNumber).HasMaxLength(50);
            entity.Property(e => e.RatingAvg).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Pending);
            entity.Property(e => e.StoreName).HasMaxLength(150);
            entity.Property(e => e.TaxNumber).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.Stores)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stores_Users");
        });

        modelBuilder.Entity<StoreStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StoreSta__96D4AB1701A0B50F");

            entity.ToTable("StoreStaff");

            entity.HasIndex(e => new { e.StoreId, e.UserId }, "UQ_StoreStaff").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Pending);

            entity.HasOne(d => d.Store).WithMany(p => p.StoreStaffs)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreStaff_Stores");

            entity.HasOne(d => d.User).WithMany(p => p.StoreStaffs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreStaff_Users");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallets__84D4F90EC8015006");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Balance).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("VND");
            entity.Property(e => e.OwnerType).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Active);
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WalletTr__C19608541DF87B9D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ProviderTxnId).HasMaxLength(120);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue(StatusValue.Success);
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Order).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_WalletTransactions_Orders");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WalletTransactions_Wallets");
        });
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasMany(e => e.Slots)
                  .WithOne(e => e.Warehouse)
                  .HasForeignKey(e => e.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Store)
                  .WithMany(e => e.Warehouses)
                  .HasForeignKey(e => e.StoreId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(x => new { x.StoreId, x.Name });

            entity.HasOne(e => e.Address)
                  .WithMany()
                  .HasForeignKey(e => e.AddressRefId);
        });
        modelBuilder.Entity<WarehouseSlot>(entity =>
        {
            entity.HasIndex(x => new { x.WarehouseId, x.Code })
            .IsUnique();

            entity.HasIndex(x => new { x.WarehouseId, x.Status });
        });
        modelBuilder.Entity<SlotReservation>(b =>
        {
            b.HasIndex(x => new { x.WarehouseSlotId, x.ExpiresAt });

            b.HasIndex(x => x.OrderId);

            b.HasOne(x => x.WarehouseSlot)
                .WithMany() 
                .HasForeignKey(x => x.WarehouseSlotId)
                .OnDelete(DeleteBehavior.NoAction);

            
        });
        modelBuilder.Entity<KycSubmission>(e =>
        {
            e.HasIndex(x => new { x.Status, x.SubmittedAt }); 
            e.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
            e.Property(x => x.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<KycDocument>(e =>
        {
            e.HasIndex(x => x.KycSubmissionId);
            e.HasOne(x => x.KycSubmission)
                .WithMany(s => s.Documents)
                .HasForeignKey(x => x.KycSubmissionId);
            e.Property(x => x.DocType).HasMaxLength(40);
            e.Property(x => x.FilePath).HasMaxLength(512);
        });
        modelBuilder.SeedingRoles();
        modelBuilder.SeedingStoreData();
        modelBuilder.SeedingCategoryData();
        modelBuilder.SeedingAdminData();
        modelBuilder.SeedingDataForStore();
        modelBuilder.SeedingDashboards();
        base.OnModelCreating(modelBuilder);
       
    }
    
}
