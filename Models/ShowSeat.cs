using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class ShowSeat
{
    public long ID { get; set; }
    public string? ShowSeatStatus { get; set; }
    public long? ShowTimeID { get; set; }
    [ForeignKey("ShowTimeID")]
    public virtual ShowTime? ShowTime { get; set; }
    public long? SeatID { get; set; }
    [ForeignKey("SeatID")]
    public virtual Seat? Seat { get; set; }
}
