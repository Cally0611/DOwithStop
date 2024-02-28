using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOwithStop.Models
{
    public partial class AllMachine
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
