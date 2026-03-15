using CTPS.API.Models;
using Microsoft.EntityFrameworkCore;
namespace CTPS.API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}

    public virtual DbSet<PassType> PassTypes { get; set; }
    public virtual DbSet<TransportMode> TransportModes { get; set; }
    public virtual DbSet<Trip> Trips { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserPass> UserPasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PassType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passtypes_pkey");

            entity.HasMany(d => d.TransportModes).WithMany(p => p.PassTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "PasstypeTransportmode",
                    r => r.HasOne<TransportMode>().WithMany()
                        .HasForeignKey("TransportModeId")
                        .HasConstraintName("passtype_transportmodes_transport_mode_id_fkey"),
                    l => l.HasOne<PassType>().WithMany()
                        .HasForeignKey("PassTypeId")
                        .HasConstraintName("passtype_transportmodes_pass_type_id_fkey"),
                    j =>
                    {
                        j.HasKey("PassTypeId", "TransportModeId").HasName("passtype_transportmodes_pkey");
                        j.ToTable("passtype_transportmodes");
                        j.IndexerProperty<int>("PassTypeId").HasColumnName("pass_type_id");
                        j.IndexerProperty<int>("TransportModeId").HasColumnName("transport_mode_id");
                    });
        });

        modelBuilder.Entity<TransportMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transportmodes_pkey");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trips_pkey");

            entity.Property(e => e.ValidatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.UserPass).WithMany(p => p.Trips)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("trips_user_pass_id_fkey");

            entity.HasOne(d => d.ValidatedByNavigation).WithMany(p => p.Trips)
                .HasConstraintName("trips_validated_by_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserPass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userpasses_pkey");

            entity.Property(e => e.PurchaseDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PassType).WithMany(p => p.UserPasses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("userpasses_pass_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserPasses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("userpasses_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}