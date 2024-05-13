using DOwithStop.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DOwithStop.Data
{
    public partial class OMplannerDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

    
        public OMplannerDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<FinishingAction> FinishingActions { get; set; }

        public DbSet<FinishingReason> FinishingReasons { get; set; }

        public DbSet<FinishingReasonUser> FinishingReasonUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("OMplannerDB"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<FinishingAction>(entity => entity.HasNoKey());
   
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }   
}
