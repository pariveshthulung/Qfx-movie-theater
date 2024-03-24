using System.ComponentModel.DataAnnotations.Schema;
using QFX.Constants;
using QFX.Manager.Interface;
using QFX.Migrations;

namespace QFX.Models;

public class Seat :ISoftDelete
{
    public long ID { get; set; }
    public string?  SeatName { get; set; }
    
    public string? SeatStatus { get; set; } = SeatStatusConstants.Active;
    public string? SeatType { get; set; } = SeatTypeConstants.Platinum;
    public long AudiID { get; set; }
    [ForeignKey("AudiID")]
    public virtual Audi? Audi { get; set; }
    public bool IsDeleted { get; set; } = false;
}
