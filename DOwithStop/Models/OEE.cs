namespace DOwithStop.Models
{
    public class OEE
    {
       // public int OeeID { get; set; }

        public int OeeMachineId { get; set; }

        //public int OeeUptime { get; set; }

        //public int OeeTimeTilNow  { get; set; }

        //public decimal OeeDownTime  { get; set; }

        //public decimal OeeTotalSheet  { get; set; }

        public decimal OeeAvailability { get; set; }

        public decimal OeePerformance { get; set; }
        
        public decimal OeeInPercentage 
        {
            get
            {
                return (OeeAvailability * OeePerformance * OeeQuality) / 10000;
            }
        
        }

        public decimal OeeQuality 
        { 
            get
            {
                return 100;
            } 
        }

        public decimal OeeTarget { get; set; }

    }
}
