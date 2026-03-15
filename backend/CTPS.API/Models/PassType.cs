using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Models;

[Table("passtypes")]
public partial class PassType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("validity_days")]
    public int ValidityDays { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Column("max_trips_per_day")]
    public int? MaxTripsPerDay { get; set; }

    [InverseProperty("PassType")]
    public virtual ICollection<UserPass> UserPasses { get; set; } = new List<UserPass>();

    [ForeignKey("PassTypeId")]
    [InverseProperty("PassTypes")]
    public virtual ICollection<TransportMode> TransportModes { get; set; } = new List<TransportMode>();
}
