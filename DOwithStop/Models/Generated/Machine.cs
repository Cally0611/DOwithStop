using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Models;

[Table("Machine")]
public partial class Machine
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string MachineName { get; set; } = null!;

    [InverseProperty("Machine")]
    public virtual ICollection<AllMachines> AllMachines { get; } = new List<AllMachines>();
}
