using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOwithStop.Models
{
    [Table("FinishingReason")]
    public class FinishingReason
    {
        [Key]
        public int FinishSRID { get; set; }
        public required string FinishReason { get; set; } 

    }
}
