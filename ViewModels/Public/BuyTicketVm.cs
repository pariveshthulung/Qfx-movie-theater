using QFX.Models;

namespace QFX.ViewModels.PublicVm;

public class BuyTicketVm
{
    public List<Show>? Shows { get; set; }
    public List<ShowSeat>? ShowSeats { get; set; }
    public ShowSeat? ShowSeat { get; set; }
}
