namespace DOwithStop.Models
{
    public class StopReason
    {
        //public int OeeMachine { get;set; }
        public int MachineID { get; set; }
     

        public int StopDownTime { get; set; } 
        
       // public string Stop_MReason { get; set; }

        public string? StopReasonName { get; set; }

        public string? SubSRName { get; set; }
    

        //public string Stop_SReason { get; set; }
    }
}
