using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.Reflection.Metadata;
using System.Security.Claims;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace HBookWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookingController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeproperties: "Hotel,User");
            booking.Hotel = _unitOfWork.Hotel.Get(h => h.Id == booking.Hotel.Id, includeproperties: "Amenities");

            if (booking.RoomNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableRoomNumbers = GetAvailableRoomNumbersINHotel(booking.HotelId);

                booking.AvailableRooms = _unitOfWork.Room.GetAll(r => r.HotelId == booking.HotelId && availableRoomNumbers.Any(n => n == r.RoomNumber));
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

            Booking bookingFromDb = _unitOfWork.Booking.Get(b => b.Id == id, includeproperties:"Hotel");

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


            table.ResetCells(2, 4);

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
            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            Booking booking = new Booking
            {
                UserId = userId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HotelId = HotelId,
                CheckInDate = checkInDate,
                Nights = Nights,
                Hotel = _unitOfWork.Hotel.Get(h => h.Id == HotelId, includeproperties: "Amenities"),
                CheckOutDate = checkInDate.AddDays(Nights),
            };
            booking.TotalCost = booking.Hotel.Price * Nights;

            return View(booking);
        }

        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {

            var hotel = _unitOfWork.Hotel.Get(h => h.Id == booking.HotelId);
            booking.TotalCost = hotel.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            //
            var roomList = _unitOfWork.Room.GetAll().ToList();
            var bookings = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved
            || b.Status == SD.StatusCheckedIn).ToList();

            
                int roomsAvailable = SD.HotelRoomsAvailable_Count(hotel.Id, roomList, bookings, booking.CheckInDate, booking.Nights);
                if (roomsAvailable == 0)
                {
                    TempData["error"] = "Room has been sold out.";
                return RedirectToAction(nameof(FinalizeBooking), new { HotelId = booking.HotelId, CheckIn = booking.CheckInDate.ToString("yyyy-MM-dd"), Nights = booking.Nights });
                }
            //

            _unitOfWork.Booking.Add(booking);

            _unitOfWork.Save();

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

            _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public IActionResult BookingConfirmation(int bookingId)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeproperties: "Hotel,User");

            var service = new SessionService();
            Session session = service.Get(booking.StripeSessionId);

            if (booking.Status == SD.StatusPending)
            {
                _unitOfWork.Booking.UpdateBookingStatus(booking.Id, SD.StatusApproved, 0);
                _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, booking.StripeSessionId, session.PaymentIntentId);
                _unitOfWork.Save();
            }


            return View(bookingId);
        }

        #region API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> bookings;
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                bookings = _unitOfWork.Booking.GetAll(b => b.UserId == userId, includeproperties: "Hotel,User");
            }
            else
            {
                bookings = _unitOfWork.Booking.GetAll(includeproperties: "Hotel,User");
            }
            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status == status);
            }
            return Json(new { data = bookings });
        }

        #endregion

        private List<int> GetAvailableRoomNumbersINHotel(int hotelId)
        {
            List<int> availableRoomNumbers = new List<int>();

            var roomsInHotel = _unitOfWork.Room.GetAll(r => r.HotelId == hotelId);

            var checkedInRooms = _unitOfWork.Booking.GetAll(b => b.HotelId == hotelId && b.Status == SD.StatusCheckedIn).Select(b => b.RoomNumber);

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
        public IActionResult CheckIN (Booking booking)
        {
            _unitOfWork.Booking.UpdateBookingStatus(booking.Id, SD.StatusCheckedIn, booking.RoomNumber);
            _unitOfWork.Save();
            TempData["success"] = "Booking checked in successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckOut (Booking booking)
        {
            _unitOfWork.Booking.UpdateBookingStatus(booking.Id, SD.StatusCompleted, booking.RoomNumber);
            _unitOfWork.Save();
            TempData["success"] = "Booking checked out successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking (Booking booking)
        {
            _unitOfWork.Booking.UpdateBookingStatus(booking.Id, SD.StatusCancelled, 0);
            _unitOfWork.Save();
            TempData["success"] = "Booking cancelled successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }
    }
}
