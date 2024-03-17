using System.ComponentModel.DataAnnotations.Schema;
using QFX.Entity;

namespace QFX.Models;

public class TicketNotify
{
    public long ID { get; set; }
    public long MovieID { get; set; }
    [ForeignKey("MovieID")]
    public virtual Movie? Movie { get; set; }
    public long UserID { get; set; }
    [ForeignKey("UserID")]
    public virtual User? User { get; set; }
}
