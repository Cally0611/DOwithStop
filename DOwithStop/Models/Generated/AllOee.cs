using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Models;

[Keyless]
[Table("AllOee")]
public partial class AllOee
{
    public int AllOeeId { get; set; }

    public int LocalOeeId { get; set; }

    [Column("MachineID")]
    public int MachineId { get; set; }

    public int OeeTimeTilNow { get; set; }

    public int OeeUptime { get; set; }

    public int OeeTotalSheets { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OeeDateTime { get; set; }
}
