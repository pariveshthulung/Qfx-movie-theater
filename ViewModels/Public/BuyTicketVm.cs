﻿using QFX.Models;

namespace QFX.ViewModels.PublicVm;

public class BuyTicketVm
{
    public Show? Show { get; set; }
    public List<ShowDate>? ShowDates { get; set; }
    //send showTimeID to checkout
    public long ShowTimeID { get; set; }
}
