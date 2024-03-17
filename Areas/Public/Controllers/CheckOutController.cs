using System.Collections;
using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.CheckOutVm;
using Stripe;
using Stripe.Checkout;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QFX.ViewModels.Reservation;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[Authorize]


public class CheckOut : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;
    private readonly INotyfService _notifyService;

    public CheckOut(ApplicationDbContext context, ICurrentUserProvider currentUserProvider, INotyfService notyfService)
    {
        _context = context;
        _currentUser = currentUserProvider;
        _notifyService = notyfService;

    }

    // public IActionResult Post([FromBody] PostVm vm)
    // {
    //     if (vm == null)
    //     {
    //         return BadRequest("Invalid request data");
    //     }
    //     return RedirectToAction("Index", "CheckOut", vm);
    //     // return Index(vm);
    // }
    // public IActionResult Index(PostVm vm)
    // {
    //     //  var vm2= new IndexVm
    //     // {
    //     //     PlatinumPrice = 200,
    //     //     PremiumPrice = 400,
    //     //     ShowTimeID = vm.ShowTimeID,
    //     //     ShowID = vm.ShowID,
    //     //     ShowSeats = _context.ShowSeats.Where(x => vm.SeatID.Contains(x.ID)).Include(x => x.Seat).ToList()
    //     // };

    //     return View(vm);
    // }

    public async Task<IActionResult> Index([FromBody] IndexVm vm)
    {
        try
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var currentUserID = _currentUser.GetCurrentUserId();
            var reservationExist = await _context.Reservations.Where(x => x.UserID == currentUserID && x.ShowTimeID == vm.ShowTimeID).FirstOrDefaultAsync();
            long reservationID;
            if (reservationExist != null)
            {
                reservationID = reservationExist.ID;
            }
            else
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
                SuccessUrl = domain + $"Public/CheckOut/ReserveConfirmation?reservationID={reservationID}&selectedSeat={vm.SeatID}",
                CancelUrl = domain + $"Public/CkeckOut/ReservationFailed?reservationID={reservationID}",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };
            // foreach (var seat in vm.ShowSeats)
            // {
            //     if (seat.Seat.SeatType == "Platinum")
            //     {
            //         vm.PlatinumQty++;
            //     }
            //     if (seat.Seat.SeatType == "Premium")
            //     {
            //         vm.PremiumQty++;
            //     }
            // }
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
            foreach (var id in vm.SeatID)
            {
                var reservationSeat = new ReservationSeat
                {
                    ReservationID = reservationID,
                    ShowSeatID = id,
                    PaymentStatus = PaymentStatusContants.Pending
                };
                _context.ReservationSeats.Add(reservationSeat);
                await _context.SaveChangesAsync();
            }

            // TempData["selectSeatTemp"] = vm.SeatID;

            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            // add updateSessionID
            UpdateSessionID(session.Id, reservationID);
            // Response.Headers.Add("Location", session.Url);
            tx.Complete();
            // return new StatusCodeResult(303);
            return Json(new
            {
                redirect = session.Url,
                success = true
            });


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
                var reservationSeat = _context.ReservationSeats.Where(x => x.ReservationID == reservationID).ToList();
                var seatIds = _context.ReservationSeats.Where(x => x.ReservationID == reservationID).Select(x => x.ShowSeatID).ToList();
                var showSeats = _context.ShowSeats.Where(x => seatIds.Contains(x.ID)).ToList();
                foreach (var reservationSeat1 in reservationSeat)
                {
                    reservationSeat1.PaymentStatus = PaymentStatusContants.Paid;
                    await _context.SaveChangesAsync();
                }
                foreach (var showSeat in showSeats)
                {
                    showSeat.ShowSeatStatus = SeatStatusConstants.SoldOut;
                    await _context.SaveChangesAsync();
                }
            }
            tx.Complete();
            // var vm = new ReservationVm();
            // vm.TicketLink = reservationID+".pdf";
            return RedirectToAction(nameof(Index),nameof(Public));
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    public IActionResult ReservationFailed(long reservationID)
    {
        try
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            // var reservationExist = _context.Reservations.Where(x=>x.ID==reservationID).FirstOrDefault();
            var reservationSeat = _context.ReservationSeats.Where(x => x.ReservationID == reservationID).ToList();
            _context.RemoveRange(reservationSeat);
            _context.SaveChanges();
            tx.Complete();
            _notifyService.Error("Error!!! Please try again.");
            return RedirectToAction(nameof(Index), nameof(Public));
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    // [Obsolete]
    [HttpGet]
    public IActionResult DownloadTicket(long reservationID)
    {

        var reservationSeat = _context.ReservationSeats.Where(x => x.ReservationID == reservationID)
        .Include(x => x.Reservation)
        .Include(x => x.ShowSeat)
            .ThenInclude(y => y.Seat)
                .ThenInclude(z => z.Audi)
                    .ThenInclude(a => a.Location)
        .Include(x => x.ShowSeat)
            .ThenInclude(y => y.ShowTime)
                .ThenInclude(y => y.ShowDate)
        .ToList();
      
      Document document=  Document.Create(container =>
       {
           foreach (var ticket in reservationSeat)
           {
               container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()

                        .Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("QFX Cinema").SemiBold().FontSize(46);
                            text.EmptyLine();

                            text.Span(ticket.ShowSeat.Seat.Audi.Location.CityName);
                            text.EmptyLine();

                            text.Span("Entrance Pass");
                        });
                    // .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            var time = ticket.ShowSeat.ShowTime.Time.ToString("hh mm tt");
                            x.Item().Text("Show Date: " + ticket.ShowSeat.ShowTime.ShowDate.Date.ToString("ddd dd MMM "));
                            x.Item().Text("Show Time: " + time);
                            x.Item().Text("Seat no: " + ticket.ShowSeat.Seat.SeatName);
                            x.Item().Text("Seat Type: " + ticket.ShowSeat.Seat.SeatType);


                            // x.Item().Text(text =>
                            // {
                            // text.Span("Audi: "+ ticket.ShowSeat.Seat.Audi.Name);
                            // text.Span("Seat no: " + ticket.ShowSeat.Seat.SeatName);

                            // text.Span("Seat Type: "+ ticket.ShowSeat.Seat.SeatType);

                            // });
                        });


                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            // x.Span("Page ");
                            // x.CurrentPageNumber();
                        });

                });
           }
       });
    //    .GeneratePdf(reservatioinID + ".pdf");
        byte[] pdfBytes = document.GeneratePdf();
        MemoryStream ms = new MemoryStream(pdfBytes);
        return new FileStreamResult(ms, "application/pdf");
    }
    public void UpdateSessionID(string sessionID, long reservationID)
    {
        var reservation = _context.Reservations.Where(x => x.ID == reservationID).FirstOrDefault();
        reservation.SessionID = sessionID;
        _context.SaveChanges();
    }

}
