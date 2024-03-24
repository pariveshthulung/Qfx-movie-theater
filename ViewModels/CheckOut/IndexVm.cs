using System.ComponentModel.DataAnnotations.Schema;
using QFX.Models;

namespace QFX.ViewModels.CheckOutVm;

public class IndexVm
{
     public List<long>? SeatID { get; set; }
    public long ShowTimeID { get; set; }
    public long ShowID { get; set; }
    public List<ShowSeat>? ShowSeats { get; set; }
    public int PremiumQty { get; set; }
    public int PlatinumQty { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal PremiumPrice { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal PlatinumPrice { get; set; }
}
