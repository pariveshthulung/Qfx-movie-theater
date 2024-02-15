using System.ComponentModel.DataAnnotations.Schema;
namespace QFX.Models;

public class ReservationSeat
{
    public long ID { get; set; }
    public long ReservationID { get; set; }
    [ForeignKey("ReservationID")]
    public virtual Reservation? Reservation { get; set; }
    public long ShowSeatID { get; set; }
    [ForeignKey("ShowSeatID")]
    public virtual ShowSeat? ShowSeat { get; set; }
}
