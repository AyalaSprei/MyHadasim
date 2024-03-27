using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoronaClinic.Models;

public partial class CoronaClinicContext : DbContext
{
    public CoronaClinicContext()
    {
    }

    public CoronaClinicContext(DbContextOptions<CoronaClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Creator> Creators { get; set; }

    public virtual DbSet<Illness> Illnesses { get; set; }

    public virtual DbSet<Immune> Immunes { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Creator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Creators__3213E83FDC4AD4EF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Illness>(entity =>
        {
            entity.HasKey(e => e.IllnessId).HasName("PK__Illnesse__A749CA3A54D69958");

            entity.HasIndex(e => new { e.IllnessId, e.MemberId }, "UC_Illnesses").IsUnique();

            entity.Property(e => e.IllnessId).HasColumnName("illnessId");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.NegativeDate).HasColumnName("negativeDate");
            entity.Property(e => e.PositiveDate).HasColumnName("positiveDate");

            entity.HasOne(d => d.Member).WithMany(p => p.Illnesses)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Illnesses__membe__628FA481");
        });

        modelBuilder.Entity<Immune>(entity =>
        {
            entity.HasKey(e => e.ImmuneId).HasName("PK__Immune__92A585D47937702C");

            entity.ToTable("Immune");

            entity.Property(e => e.ImmuneId).HasColumnName("ImmuneID");
            entity.Property(e => e.CreatorId).HasColumnName("creatorId");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.MemberId).HasColumnName("memberId");

            entity.HasOne(d => d.Creator).WithMany(p => p.Immunes)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Immune__creatorI__5FB337D6");

            entity.HasOne(d => d.Member).WithMany(p => p.Immunes)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Immune__memberId__5EBF139D");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Members__3213E83F49396E94");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.HomeNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("homeNumber");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("identity_number");
            entity.Property(e => e.IsImmune).HasColumnName("isImmune");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mobile_phone");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .IsUnicode(false)
                .HasColumnName("picture");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telephone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
