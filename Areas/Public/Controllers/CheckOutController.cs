using System.Collections;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.CheckOutVm;
using Stripe;
using Stripe.Checkout;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[Authorize]


public class CheckOut : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;
    public CheckOut(ApplicationDbContext context, ICurrentUserProvider currentUserProvider)
    {
        _context = context;
        _currentUser = currentUserProvider;
    }

    public IActionResult Post([FromBody] PostVm vm)
    {
        if (vm == null)
        {
            return BadRequest("Invalid request data");
        }
        return RedirectToAction("Index", "CheckOut", vm);
        // return Index(vm);
    }
    public IActionResult Index(PostVm vm)
    {
        //  var vm2= new IndexVm
        // {
        //     PlatinumPrice = 200,
        //     PremiumPrice = 400,
        //     ShowTimeID = vm.ShowTimeID,
        //     ShowID = vm.ShowID,
        //     ShowSeats = _context.ShowSeats.Where(x => vm.SeatID.Contains(x.ID)).Include(x => x.Seat).ToList()
        // };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> IndexAsync(IndexVm vm)
    {
        try
        {

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var currentUserID = _currentUser.GetCurrentUserId();
            var reservationExist = await _context.Reservations.Where(x => x.UserID == currentUserID && x.ShowTimeID == vm.ShowTimeID).FirstOrDefaultAsync();
            var reservationID = reservationExist.ID;

            if (reservationExist == null)
            {
                var reservation = new Reservation
                {
                    ShowTimeID = vm.ShowTimeID,
                    UserID = (long)currentUserID,
                    ShowID = vm.ShowID
                };
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                reservationID = reservation.ID;
            }

            var domain = "http://localhost:5171/";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"Public/CheckOut/ReserveConfirmation?reservationID={reservationID}",
                CancelUrl = domain + $"Public/Public/BuyTicket",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var seat in vm.ShowSeats)
            {
                if (seat.Seat.SeatType == "Platinum")
                {
                    vm.PlatinumQty++;
                }
                if (seat.Seat.SeatType == "Premium")
                {
                    vm.PremiumQty++;
                }
            }
            if (vm.PremiumQty > 0)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ((long?)(vm.PremiumPrice * 100)),
                        Currency = "INR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Premium"
                        }
                    },
                    Quantity = vm.PremiumQty
                };
                options.LineItems.Add(sessionLineItem);

            }
            if (vm.PlatinumQty > 0)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ((long?)(vm.PlatinumPrice * 100)),
                        Currency = "INR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Platinum"
                        }
                    },
                    Quantity = vm.PlatinumQty
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            // add updateSessionID
            UpdateSessionID(session.Id, reservationID);
            Response.Headers.Add("Location", session.Url);
            tx.Complete();
            return new StatusCodeResult(303);

        }
        catch (Exception e)
        {
            return Content(e.Message);
        }

    }

    public async Task<IActionResult> ReserveConfirmation(long reservationID)
    {
        try
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var reservationExist = _context.Reservations.Where(x => x.ID == reservationID).FirstOrDefault();
            var service = new SessionService();
            Session session = service.Get(reservationExist.SessionID);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                var selectedSeats = TempData["showSeats"] as ArrayList;
                foreach (var id in selectedSeats)
                {
                    var reservationSeat = new ReservationSeat
                    {
                        ReservationID = reservationID,
                        ShowSeatID = (long)id
                    };
                    _context.ReservationSeats.Add(reservationSeat);
                    await _context.SaveChangesAsync();
                }
            }
            tx.Complete();
            return View();
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
    public async void UpdateSessionID(string sessionID, long reservationID)
    {
        var reservation = await _context.Reservations.Where(x => x.ID == reservationID).FirstOrDefaultAsync();
        reservation.SessionID = sessionID;
        await _context.SaveChangesAsync();
    }

}
