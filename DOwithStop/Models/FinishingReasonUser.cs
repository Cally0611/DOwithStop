using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOwithStop.Models
{
    [Table("FinishingReasonUser")]
    public class FinishingReasonUser
    {
        [Key]
        public int FinishrsnID { get; set; }
        public DateTime FinishrsnTimeStamp { get; set; }

        [ForeignKey("FinishSRID")]
        [InverseProperty("FinishingReason")]
        public int FinishrsnText { get; set; }

        public int FinishrsnPlant { get; set; }
    }
}
