using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class DatabaseManager : DbContext
{
    public DatabaseManager()
    {
    }

    public DatabaseManager(DbContextOptions<DatabaseManager> options)
        : base(options)
    {
    }

    public virtual DbSet<BusyAppointment> BusyAppointments { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<EmptyAppointment> EmptyAppointments { get; set; }

    public virtual DbSet<Therapist> Therapists { get; set; }

    public virtual DbSet<TherapistWorkingHour> TherapistWorkingHours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='H:\\Project FullStack\\Dal\\dataBase\\dataBase.mdf';Integrated Security=True;Connect Timeout=30;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusyAppointment>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__BusyAppo__A25C5AA6F555947A");

            entity.Property(e => e.ClientId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TherapistId)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Client).WithMany(p => p.BusyAppointments)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_BusyAppointments_Clients");

            entity.HasOne(d => d.Therapist).WithMany(p => p.BusyAppointments)
                .HasForeignKey(d => d.TherapistId)
                .HasConstraintName("FK_BusyAppointments_Therapists");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0770D3536D");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TherapistId)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<EmptyAppointment>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__EmptyApp__A25C5AA6EBDA7A95");

            entity.Property(e => e.TherapistId)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Therapist).WithMany(p => p.EmptyAppointments)
                .HasForeignKey(d => d.TherapistId)
                .HasConstraintName("FK_EmptyAppointments_Therapists");
        });

        modelBuilder.Entity<Therapist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC073D49EF05");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Specialization)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TherapistWorkingHour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Therapis__3214EC07F845A6C5");

            entity.Property(e => e.TherapistId)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Therapist).WithMany(p => p.TherapistWorkingHours)
                .HasForeignKey(d => d.TherapistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TherapistWorkingHours_Therapists");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
