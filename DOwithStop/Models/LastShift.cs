using System.Data.SqlTypes;

namespace DOwithStop.Models
{
    public class LastShift
    {
        public DateTime LastShiftDate { get; set; }
     
        public int LastShiftNo { get; set; }

        public DateTime ShiftStartDT { get; set; }
        public DateTime ShiftEndDT { get; set; }

        public int MachineID { get; set; }

        public string JobName { get; set; } = null!;

        public int PReasonCode { get; set; }
        public int CReasonCode { get; set; } 






    }
}
