using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Models;

[Table("trips")]
public partial class Trip
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_pass_id")]
    public int? UserPassId { get; set; }

    [Column("validated_by")]
    public int? ValidatedBy { get; set; }

    [Column("transport_mode")]
    [StringLength(50)]
    public string TransportMode { get; set; } = null!;

    [Column("route_info")]
    public string? RouteInfo { get; set; }

    [Column("validated_at", TypeName = "timestamp without time zone")]
    public DateTime? ValidatedAt { get; set; }

    [ForeignKey("UserPassId")]
    [InverseProperty("Trips")]
    public virtual UserPass? UserPass { get; set; }

    [ForeignKey("ValidatedBy")]
    [InverseProperty("Trips")]
    public virtual User? ValidatedByNavigation { get; set; }
}
