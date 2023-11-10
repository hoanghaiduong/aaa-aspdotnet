using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace aaa_aspdotnet.src.DAL.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DailyDivision> DailyDivisions { get; set; }

    public virtual DbSet<DetailPlan> DetailPlans { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceType> DeviceTypes { get; set; }

    public virtual DbSet<Factory> Factories { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Technical> Technicals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkStatus> WorkStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=118.69.126.49;Initial Catalog=data_QuanLyThietBi;Persist Security Info=True;User ID=UserQuanLyThietBi;TrustServerCertificate=True;Password=S6f6U7G50NfO");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8E59BF282");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DailyDivision>(entity =>
        {
            entity.HasKey(e => new { e.DeviceId, e.PlanId, e.TechnicalId }).HasName("pk_DailyDivision");

            entity.ToTable("DailyDivision");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.TechnicalId).HasColumnName("TechnicalID");
            entity.Property(e => e.AfterImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BeforeImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompletedDate).HasColumnType("date");
            entity.Property(e => e.JobDescription).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.SpecificContents).HasMaxLength(500);
            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("((4))")
                .HasColumnName("StatusID");
            entity.Property(e => e.WorkDay)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Device).WithMany(p => p.DailyDivisions)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DailyDivision_Device");

            entity.HasOne(d => d.Plan).WithMany(p => p.DailyDivisions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DailyDivision_Plan");

            entity.HasOne(d => d.Status).WithMany(p => p.DailyDivisions)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DailyDivision_WorkStatus");

            entity.HasOne(d => d.Technical).WithMany(p => p.DailyDivisions)
                .HasForeignKey(d => d.TechnicalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DailyDivision_Technical");
        });

        modelBuilder.Entity<DetailPlan>(entity =>
        {
            entity.HasKey(e => new { e.DeviceId, e.PlanId }).HasName("pk_DetailPlan");

            entity.ToTable("DetailPlan");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.ExpectedDate).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.Specification).HasMaxLength(255);
            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("((4))")
                .HasColumnName("StatusID");
            entity.Property(e => e.TypePlan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('PM')");
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.Device).WithMany(p => p.DetailPlans)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DetailPlan_Device");

            entity.HasOne(d => d.Plan).WithMany(p => p.DetailPlans)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DetailPlan_Plan");

            entity.HasOne(d => d.Status).WithMany(p => p.DetailPlans)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DetailPlan_WorkStatus");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK__Devices__49E1233103C7F49A");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descriptions).HasMaxLength(255);
            entity.Property(e => e.DeviceName).HasMaxLength(255);
            entity.Property(e => e.FacId).HasColumnName("FacID");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("QRCode");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Fac).WithMany(p => p.Devices)
                .HasForeignKey(d => d.FacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Device_Factory");

            entity.HasOne(d => d.Type).WithMany(p => p.Devices)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Devices_DeviceTypes");
        });

        modelBuilder.Entity<DeviceType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Factory>(entity =>
        {
            entity.HasKey(e => e.FacId).HasName("PK__Factory__815081C831BD8E9C");

            entity.ToTable("Factory");

            entity.Property(e => e.FacId).HasColumnName("FacID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Alias)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FacName).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Factories)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Customer_Factory");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__Plans__755C22D76733A685");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.BeginDate).HasColumnType("date");
            entity.Property(e => e.Contents).HasMaxLength(255);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("date");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId)
                .HasMaxLength(255)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsActived).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Technical>(entity =>
        {
            entity.HasKey(e => e.TechnicalId).HasName("PK__Technica__F6E0649FE62EF743");

            entity.ToTable("Technical");

            entity.Property(e => e.TechnicalId).HasColumnName("TechnicalID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TechnicalName).HasMaxLength(50);
            entity.Property(e => e.Zalo)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActived).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<WorkStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__WorkStat__C8EE20433B62230C");

            entity.ToTable("WorkStatus");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
