using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Models;

[Table("transportmodes")]
[Index("Code", Name = "transportmodes_code_key", IsUnique = true)]
public partial class TransportMode
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("code")]
    [StringLength(20)]
    public string Code { get; set; } = null!;

    [ForeignKey("TransportModeId")]
    [InverseProperty("TransportModes")]
    public virtual ICollection<PassType> PassTypes { get; set; } = new List<PassType>();
}
