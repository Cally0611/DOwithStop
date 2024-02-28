using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Models;

public partial class AllMachine
{
    [Key]
    [Column("AllMachineID")]
    public int AllMachineId { get; set; }

    [Column("MachineID")]
    public int MachineId { get; set; }

    [Column("Target_Date", TypeName = "date")]
    public DateTime TargetDate { get; set; }

    [Column("Job_Name1")]
    [StringLength(100)]
    [Unicode(false)]
    public string JobName1 { get; set; } = null!;

    [Column("Job_Name2")]
    [StringLength(100)]
    [Unicode(false)]
    public string? JobName2 { get; set; }

    [Column("ParentReasonCode_FullDay")]
    [StringLength(100)]
    [Unicode(false)]
    public string ParentReasonCodeFullDay { get; set; } = null!;

    [Column("ParentReasonCode_Shift1")]
    [StringLength(100)]
    [Unicode(false)]
    public string ParentReasonCodeShift1 { get; set; } = null!;

    [Column("ParentReasonCode_Shift2")]
    [StringLength(100)]
    [Unicode(false)]
    public string ParentReasonCodeShift2 { get; set; } = null!;

    [Column("ChildReasonCode_Shift1")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ChildReasonCodeShift1 { get; set; }

    [Column("ChildReasonCode_Shift2")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ChildReasonCodeShift2 { get; set; }

    [Column("Target_data_FullDay")]
    public int TargetDataFullDay { get; set; }

    [Column("Target_data_Shift1")]
    public int TargetDataShift1 { get; set; }

    [Column("Target_data_Shift2")]
    public int TargetDataShift2 { get; set; }

    [Column("Pallette_Qty_FullDay")]
    public int PalletteQtyFullDay { get; set; }

    [Column("Pallette_Qty_Shift1")]
    public int PalletteQtyShift1 { get; set; }

    [Column("Pallette_Qty_Shift2")]
    public int PalletteQtyShift2 { get; set; }

    [Column("Serial_no")]
    [StringLength(50)]
    [Unicode(false)]
    public string SerialNo { get; set; } = null!;

    public DateTime OprTimeStamp { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string TypeofOpr { get; set; } = null!;

    [ForeignKey("MachineId")]
    [InverseProperty("AllMachines")]
    public virtual Machine Machine { get; set; } = null!;
}
