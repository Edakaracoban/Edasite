using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace edasite.data.Models;

public partial class edasiteContext : DbContext
{
    public edasiteContext()
    {
    }

    public edasiteContext(DbContextOptions<edasiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aidat> Aidats { get; set; }

    public virtual DbSet<Blok> Bloks { get; set; }

    public virtual DbSet<Daire> Daires { get; set; }

    public virtual DbSet<Site> Sites { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=edasite;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aidat>(entity =>
        {
            entity.HasKey(e => e.AidatNo);

            entity.ToTable("Aidat");

            entity.Property(e => e.OdemeDurumu).HasMaxLength(50);
            entity.Property(e => e.Tutar).HasColumnType("money");

            entity.HasOne(d => d.SiteNoNavigation).WithMany(p => p.Aidats)
                .HasForeignKey(d => d.SiteNo)
                .HasConstraintName("FK_Aidat_Site");
        });

        modelBuilder.Entity<Blok>(entity =>
        {
            entity.HasKey(e => e.BlokNo);

            entity.ToTable("Blok");

            entity.Property(e => e.BlokAdi).HasMaxLength(50);

            entity.HasOne(d => d.SiteNoNavigation).WithMany(p => p.Bloks)
                .HasForeignKey(d => d.SiteNo)
                .HasConstraintName("FK_Blok_Site");
        });

        modelBuilder.Entity<Daire>(entity =>
        {
            entity.HasKey(e => e.DaireNo);

            entity.ToTable("Daire");

            entity.HasOne(d => d.UserNoNavigation).WithMany(p => p.Daires)
                .HasForeignKey(d => d.UserNo)
                .HasConstraintName("FK_Daire_User");
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(e => e.SiteNo);

            entity.ToTable("Site");

            entity.Property(e => e.Adres).HasMaxLength(50);
            entity.Property(e => e.Bilgi).HasMaxLength(50);
            entity.Property(e => e.SiteAdi).HasMaxLength(50);

            entity.HasOne(d => d.BlokNoNavigation).WithMany(p => p.Sites)
                .HasForeignKey(d => d.BlokNo)
                .HasConstraintName("FK_Site_Blok");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserNo);

            entity.ToTable("User");

            entity.Property(e => e.AdSoyad).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Sifre).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
