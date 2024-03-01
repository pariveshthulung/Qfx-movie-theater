using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class ShowTime
{
    public long ID { get; set; }
    public DateTime Time { get; set; }
    public long ShowDateID { get; set; }
    [ForeignKey("ShowDateID")]
    public virtual ShowDate? ShowDate { get; set; }
}
