using System.ComponentModel.DataAnnotations.Schema;
using QFX.Constants;
using QFX.Migrations;

namespace QFX.Models;

public class Seat
{
    public long ID { get; set; }
    public string?  SeatName { get; set; }
    public string? SeatStatus { get; set; } = SeatStatusConstants.Active;
    public string? SeatType { get; set; } = SeatTypeConstants.Platinum;
    public long AudiID { get; set; }
    [ForeignKey("AudiID")]
    public virtual Audi? Audi { get; set; }
}
