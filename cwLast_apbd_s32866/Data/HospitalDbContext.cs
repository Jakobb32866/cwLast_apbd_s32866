using cwLast_apbd_s32866.Models;
using Microsoft.EntityFrameworkCore;

namespace cwLast_apbd_s32866.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<BedType> BedTypes { get; set; }
    public DbSet<Bed> Beds { get; set; }
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<BedAssignment> BedAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.Pesel);
            e.Property(p => p.Pesel).HasColumnType("char(11)");
            e.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            e.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Ward>(e =>
        {
            e.HasKey(w => w.Id);
            e.Property(w => w.Name).IsRequired().HasMaxLength(300);
            e.Property(w => w.Description).HasColumnType("nvarchar(max)").IsRequired();
        });

        modelBuilder.Entity<Room>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnType("varchar(4)");

            e.HasOne(r => r.Ward)
             .WithMany(w => w.Rooms)
             .HasForeignKey(r => r.WardId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BedType>(e =>
        {
            e.HasKey(bt => bt.Id);
            e.Property(bt => bt.Name).IsRequired().HasMaxLength(300);
            e.Property(bt => bt.Description).HasColumnType("nvarchar(max)").IsRequired();
        });

        modelBuilder.Entity<Bed>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.RoomId).HasColumnType("varchar(4)");

            e.HasOne(b => b.Room)
             .WithMany(r => r.Beds)
             .HasForeignKey(b => b.RoomId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(b => b.BedType)
             .WithMany(bt => bt.Beds)
             .HasForeignKey(b => b.BedTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Admission>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.PatientPesel).HasColumnType("char(11)");

            e.HasOne(a => a.Patient)
             .WithMany(p => p.Admissions)
             .HasForeignKey(a => a.PatientPesel)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(a => a.Ward)
             .WithMany(w => w.Admissions)
             .HasForeignKey(a => a.WardId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BedAssignment>(e =>
        {
            e.HasKey(ba => ba.Id);
            e.Property(ba => ba.PatientPesel).HasColumnType("char(11)");
            e.Property(ba => ba.From).HasColumnName("From");
            e.Property(ba => ba.To).HasColumnName("To");

            e.HasOne(ba => ba.Patient)
             .WithMany(p => p.BedAssignments)
             .HasForeignKey(ba => ba.PatientPesel)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ba => ba.Bed)
             .WithMany(b => b.BedAssignments)
             .HasForeignKey(ba => ba.BedId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}