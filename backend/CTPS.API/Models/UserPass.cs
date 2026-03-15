using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTPS.API.Models;

[Table("userpasses")]
[Index("PassCode", Name = "userpasses_pass_code_key", IsUnique = true)]
public partial class UserPass
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("pass_type_id")]
    public int? PassTypeId { get; set; }

    [Column("pass_code")]
    [StringLength(100)]
    public string PassCode { get; set; } = null!;

    [Column("purchase_date", TypeName = "timestamp without time zone")]
    public DateTime? PurchaseDate { get; set; }

    [Column("expiry_date", TypeName = "timestamp without time zone")]
    public DateTime ExpiryDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [ForeignKey("PassTypeId")]
    [InverseProperty("Userpasses")]
    public virtual PassType? PassType { get; set; }

    [InverseProperty("UserPass")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    [ForeignKey("UserId")]
    [InverseProperty("Userpasses")]
    public virtual User? User { get; set; }
}
