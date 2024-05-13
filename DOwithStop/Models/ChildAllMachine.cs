using System.ComponentModel.DataAnnotations.Schema;

namespace DOwithStop.Models
{
   
    public class ChildAllMachine : AllMachines
    {
     
        [Column("ActualNum")]
        public int ActualNum { get; set; }

       
        [Column("MachineName")]
        public string MachineName { get; set; }

       

        [Column("TargetPerShift")]
        public int TargetPerShift { get; set; }

       
        [Column("ParentCodePerShift")]
        public string ParentCodePerShift { get; set; }

     
        [Column("ChildCodePerShift")]
        public string ChildCodePerShift { get; set; }
    }
}
