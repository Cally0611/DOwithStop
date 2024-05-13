using DOwithStop.Models;



//using DOwithStop.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DOwithStop.Data
{
    public partial class CustomDBContext : OeedashboardContext
    {
        private readonly IConfiguration _configuration;
        public CustomDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override DbSet<AllMachines> AllMachines { get; set; }
      
        public override DbSet<Machine> Machines { get; set; }

        public override DbSet<AllOee> AllOees { get; set; }
        public override DbSet<AllOeeCalculation> AllOeeCalculations { get; set; }

        public override DbSet<AllTargetOee> AllTargetOees { get; set; }
        public DbSet<StopReason> StopReasons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DOServerDB"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllMachines>(entity =>
            {
                entity.Ignore(x => x.JobName2);
                entity.Ignore(x => x.OprTimeStamp);
                entity.Ignore(x => x.SerialNo);
                entity.Ignore(x => x.TypeofOpr);
                //entity.Ignore(x => x.ChildReasonCodeShift1);
                //entity.Ignore(x => x.ChildReasonCodeShift2);
                entity.HasOne(d => d.Machine).WithMany(p => p.AllMachines)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_AllMachines_Machine");

            });

            //modelBuilder.Entity<ChildAllMachine>(entity => entity.HasKey(x => x.AllMachineId));


            modelBuilder.Entity<StopReason>(entity => entity.HasNoKey());

        

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

       
    }
}
