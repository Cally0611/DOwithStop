using System.ComponentModel.DataAnnotations.Schema;

namespace DOwithStop.Models
{
    public class FinishingAction
    {
        public DateTime targetDate { get; set; }
        public decimal F1_Target { get; set; }

        public decimal F2_Target { get; set; }
        public  int F1_Actual { get; set; }

        public int F2_Actual { get; set; }

       
    }
}
