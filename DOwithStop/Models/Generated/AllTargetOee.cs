using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Models;
[Table("AllTargetOee")]
public partial class AllTargetOee
    {

        [Key]
        public int OeeId { get; set; }
        public int MachineId { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TargetOee { get; set; }

        public int daysPerWeek { get; set; }
    }


