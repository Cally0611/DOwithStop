using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Models;

[Keyless]
[Table("AllOeeCalculation")]
public partial class AllOeeCalculation
{
    public int OeeId { get; set; }

    public int OeeMachineId { get; set; }

    public int OeeUptime { get; set; }

    public int OeeTimeTilNow { get; set; }

    public int OeeDownTime { get; set; }

    public int OeeTotalSheet { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal OeeInPercentage { get; set; }
}
