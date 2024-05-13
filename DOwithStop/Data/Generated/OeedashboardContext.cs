using System;
using System.Collections.Generic;
using DOwithStop.Models;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Data;

public partial class OeedashboardContext : DbContext
{
    public OeedashboardContext()
    {
    }

    public OeedashboardContext(DbContextOptions<OeedashboardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllMachines> AllMachines { get; set; }

    public virtual DbSet<AllOee> AllOees { get; set; }

    public virtual DbSet<AllOeeCalculation> AllOeeCalculations { get; set; }

    public virtual DbSet<AllTargetOee> AllTargetOees { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\;Initial Catalog= OEEDashboard;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllMachines>(entity =>
        {
            entity.HasOne(d => d.Machine).WithMany(p => p.AllMachines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AllMachines_Machine");
        });

        modelBuilder.Entity<AllOee>(entity =>
        {
            entity.Property(e => e.AllOeeId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<AllTargetOee>(entity =>
        {
            entity.Property(e => e.OeeId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
