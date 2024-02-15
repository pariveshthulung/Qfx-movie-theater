using System.ComponentModel.DataAnnotations.Schema;
using QFX.Entity;

namespace QFX.Models;

public class Reservation
{
    public long ID { get; set; }
    public long ShowID { get; set; }
    [ForeignKey("ShowID")]
    public virtual Show? Show { get; set; }
    public long UserID { get; set; }
    [ForeignKey("UserID")]
    public virtual User? User { get; set; }
}
