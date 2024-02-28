using System;
using System.Collections.Generic;
using DOwithStop.Models;

//using DOwithStop.Models.Generated;
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

    public virtual DbSet<AllMachine> AllMachines { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name= DOServerDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllMachine>(entity =>
        {
            entity.HasOne(d => d.Machine).WithMany(p => p.AllMachines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AllMachines_Machine");
        });

      

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
