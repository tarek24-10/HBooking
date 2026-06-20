using HBook.Application.Common.Utility;
using HBook.Application.Contract;
using HBook.Application.Services.Interface;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.Security.Claims;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace HBookWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public BookingController(IBookingService bookingService, IHotelService hotelService,
            IRoomService roomService, IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _bookingService = bookingService;
            _hotelService = hotelService;
            _roomService = roomService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _emailService = emailService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId);
            if (booking.RoomNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableRoomNumbers = GetAvailableRoomNumbersINHotel(booking.HotelId);

                booking.AvailableRooms = _roomService.GetAllRooms().Where(r => r.HotelId == booking.HotelId && availableRoomNumbers.Any(n => n == r.RoomNumber));
            }

            return View(booking);
        }

        [HttpPost]
        public IActionResult GenerateInvoice(int id, string downloadType)
        {
            string basePath = _webHostEnvironment.WebRootPath;

            string dataPath = basePath + @"/exports/BookingDetails.docx";

            WordDocument document = new();

            using FileStream fileStream = new(dataPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            document.Open(fileStream, FormatType.Automatic);

            Booking bookingFromDb = _bookingService.GetBookingById(id);

            TextSelection textSelection = document.Find("xx_customer_name", false, true);
            WTextRange wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.Name;

            textSelection = document.Find("xx_customer_email", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.Email;

            textSelection = document.Find("xx_customer_phone", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.PhoneNumber;

            textSelection = document.Find("xx_payment_date", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.PaymentDate.ToShortDateString();

            textSelection = document.Find("xx_BOOKING_DATE", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = "BOOKING DATE - " + bookingFromDb.BookingDate.ToShortDateString();


            textSelection = document.Find("xx_checkin_date", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.CheckInDate.ToShortDateString();


            textSelection = document.Find("xx_checkout_date", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.CheckOutDate.ToShortDateString();


            textSelection = document.Find("xx_booking_total", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = bookingFromDb.TotalCost.ToString("c");

            textSelection = document.Find("xx_BOOKING_NUMBER", false, true);
            wTextRange = textSelection.GetAsOneRange();
            wTextRange.Text = "BOOKING NUMBER - " + bookingFromDb.Id.ToString();

            WTable table = new(document);

            table.TableFormat.Borders.LineWidth = 1f;
            table.TableFormat.Borders.Color = Color.Black;
            table.TableFormat.Paddings.Top = 7f;
            table.TableFormat.Paddings.Bottom = 7f;
            table.TableFormat.Borders.Horizontal.LineWidth = 1f;

            int rows = bookingFromDb.RoomNumber > 0 ? 3 : 2;
            table.ResetCells(rows, 4);

            WTableRow row0 = table.Rows[0];
            row0.Cells[0].AddParagraph().AppendText("NIGHTS");
            row0.Cells[0].Width = 80;
            row0.Cells[1].AddParagraph().AppendText("HOTEL");
            row0.Cells[1].Width = 220;
            row0.Cells[2].AddParagraph().AppendText("PRICE PER NIGHT");
            row0.Cells[3].AddParagraph().AppendText("TOTAL");
            row0.Cells[3].Width = 80;


            WTableRow row1 = table.Rows[1];
            row1.Cells[0].AddParagraph().AppendText(bookingFromDb.Nights.ToString());
            row1.Cells[0].Width = 80;
            row1.Cells[1].AddParagraph().AppendText(bookingFromDb.Hotel.Name);
            row1.Cells[1].Width = 220;
            row1.Cells[2].AddParagraph().AppendText(bookingFromDb.Hotel.Price.ToString("c"));
            row1.Cells[3].AddParagraph().AppendText(bookingFromDb.TotalCost.ToString("c"));
            row1.Cells[3].Width = 80;

            if (bookingFromDb.RoomNumber > 0)
            {
                WTableRow row2 = table.Rows[2];
                row1.Cells[0].Width = 80;
                row2.Cells[1].AddParagraph().AppendText("Room Number - " + bookingFromDb.RoomNumber.ToString());
                row1.Cells[1].Width = 220;
                row1.Cells[3].Width = 80;
            }

            WTableStyle tableStyle = document.AddTableStyle("CustomStyle");

            tableStyle.TableProperties.RowStripe = 1;
            tableStyle.TableProperties.ColumnStripe = 2;
            tableStyle.TableProperties.Paddings.Top = 2;
            tableStyle.TableProperties.Paddings.Bottom = 1;
            tableStyle.TableProperties.Paddings.Left = 5.4f;
            tableStyle.TableProperties.Paddings.Right = 5.4f;

            ConditionalFormattingStyle firstRowStyle = tableStyle.ConditionalFormattingStyles.Add(ConditionalFormattingType.FirstRow);
            firstRowStyle.CharacterFormat.Bold = true;
            firstRowStyle.CharacterFormat.TextColor = Color.FromArgb(255, 255, 255, 255);
            firstRowStyle.CellProperties.BackColor = Color.Black;

            table.ApplyStyle("CustomStyle");

            TextBodyPart bodyPart = new(document);
            bodyPart.BodyItems.Add(table);

            document.Replace("<ADDTABLEHERE>", bodyPart, false, false);


            using DocIORenderer renderer = new();
            MemoryStream stream = new();

            if (downloadType == "word")
            {
                document.Save(stream, FormatType.Docx);
                stream.Position = 0;

                return File(stream, "application/docx", "BookingDetails.docx");
            }
            else
            {
                PdfDocument pdfDocument = renderer.ConvertToPDF(document);
                pdfDocument.Save(stream);
                stream.Position = 0;
                return File(stream, "application/pdf", "BookingDetails.pdf");
            }
        }

        [Authorize]
        public IActionResult FinalizeBooking(int HotelId, string CheckIn, int Nights)
        {
            var checkInDate = DateOnly.Parse(CheckIn);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            Booking booking = new Booking
            {
                UserId = userId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HotelId = HotelId,
                CheckInDate = checkInDate,
                Nights = Nights,
                Hotel = _hotelService.GetHotelById(HotelId),
                CheckOutDate = checkInDate.AddDays(Nights),
            };
            booking.TotalCost = booking.Hotel.Price * Nights;

            return View(booking);
        }

        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {

            var hotel = _hotelService.GetHotelById(booking.HotelId);
            booking.TotalCost = hotel.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            //
            if (!_hotelService.IsAvailableByDate(hotel.Id, booking.Nights, booking.CheckInDate))
            {
                TempData["error"] = "Room has been sold out.";
                return RedirectToAction(nameof(FinalizeBooking),
                new
                {
                    HotelId = booking.HotelId,
                    CheckIn = booking.CheckInDate.ToString("yyyy-MM-dd"),
                    Nights = booking.Nights
                });
            }
            //

            _bookingService.CreateBooking(booking);


            var domain = $"{Request.Scheme}://{Request.Host.Value}";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                SuccessUrl = domain + $"/Booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = domain + $"/Booking/FinalizeBooking?HotelId={booking.HotelId}&CheckIn={booking.CheckInDate}&Nights={booking.Nights}",
                Mode = "payment",
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100), // Convert to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = hotel.Name,
                        Description = $"Booking for {booking.Nights} nights at {hotel.Name}",
                    },
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session = service.Create(options);

            _bookingService.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public IActionResult BookingConfirmation(int bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId);

            var service = new SessionService();
            Session session = service.Get(booking.StripeSessionId);

            if (booking.Status == SD.StatusPending)
            {
                _bookingService.UpdateBookingStatus(booking.Id, SD.StatusApproved, 0);
                _bookingService.UpdateStripePaymentId(booking.Id, booking.StripeSessionId, session.PaymentIntentId);
                _emailService.SendEmailAsync(booking.Email, "<p>Booking Confirmation - HBook", "Your Booking has been confirmed. Booking Id - " + booking.Id + "</p>").GetAwaiter().GetResult();
            }


            return View(bookingId);
        }

        #region API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> bookings;

            string userId = "";
            if (string.IsNullOrEmpty(status))
            {
                status = "";
            }

            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            bookings = _bookingService.GetAllBookings(userId, status);

            return Json(new { data = bookings });
        }

        #endregion

        private List<int> GetAvailableRoomNumbersINHotel(int hotelId)
        {
            List<int> availableRoomNumbers = new List<int>();

            var roomsInHotel = _roomService.GetAllRooms().Where(r => r.HotelId == hotelId);

            var checkedInRooms = _bookingService.GetCheckedInRoomNumbers(hotelId);

            foreach (var room in roomsInHotel)
            {
                if (!checkedInRooms.Contains(room.RoomNumber))
                {
                    availableRoomNumbers.Add(room.RoomNumber);
                }
            }

            return availableRoomNumbers;
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckIN(Booking booking)
        {
            _bookingService.UpdateBookingStatus(booking.Id, SD.StatusCheckedIn, booking.RoomNumber);
            TempData["success"] = "Booking checked in successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckOut(Booking booking)
        {
            _bookingService.UpdateBookingStatus(booking.Id, SD.StatusCompleted, booking.RoomNumber);
            TempData["success"] = "Booking checked out successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(Booking booking)
        {
            _bookingService.UpdateBookingStatus(booking.Id, SD.StatusCancelled, 0);
            TempData["success"] = "Booking cancelled successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }
    }
}
