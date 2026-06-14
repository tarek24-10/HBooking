using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBookWeb.Models;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Presentation;
using System.Diagnostics;

namespace HBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                HotelList = _unitOfWork.Hotel.GetAll(includeproperties: "Amenities"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Nights = 1
            };
            return View(homeVM);
        }

        [HttpPost]

        public IActionResult GetHotelByDate (int nights, DateOnly checkInDate)
        {
            //Thread.Sleep(250);
            var hotelList = _unitOfWork.Hotel.GetAll(includeproperties: "Amenities").ToList();
            var roomList = _unitOfWork.Room.GetAll().ToList();
            var bookings = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved 
            || b.Status == SD.StatusCheckedIn).ToList();

            foreach(var hotel in hotelList)
            {
                int roomsAvailable = SD.HotelRoomsAvailable_Count(hotel.Id, roomList, bookings, checkInDate, nights);
                if (roomsAvailable == 0)
                {
                    hotel.IsAvailable = false;
                }
            }

            HomeVM homeVM = new HomeVM()
            {
                HotelList = hotelList,
                CheckInDate = checkInDate,
                Nights = nights
            };

            return PartialView("_HotelList" ,homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePPTExport(int id)
        {
            var hotelFromDb = _unitOfWork.Hotel.Get(h => h.Id == id, includeproperties: "Amenities");

            if(hotelFromDb == null)
            {
                return RedirectToAction(nameof(Error));
            }

            var basePath = _webHostEnvironment.WebRootPath;

            var filePath = basePath + @"/exports/ExportHotelDetails.pptx";

            using IPresentation presentation = Presentation.Open(filePath);

            ISlide slide = presentation.Slides[0];

            IShape shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtHotelName") as IShape;

            if(shape is not null)
            {
                shape.TextBody.Text = hotelFromDb.Name;
            }

            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtHotelDescription") as IShape;

            if(shape is not null)
            {
                shape.TextBody.Text = hotelFromDb.Description;
            }

            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtOccupancy") as IShape;

            if(shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy : {0} adults", hotelFromDb.Occupancy.ToString());
            }

            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtHotelSize") as IShape;

            if(shape is not null)
            {
                shape.TextBody.Text = string.Format("Room Size : {0} sqft", hotelFromDb.Sqft.ToString());
            }

            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtPricePerNight") as IShape;

            if(shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night" ,hotelFromDb.Price.ToString("c"));
            }

            //
            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "txtHotelAmenitiesHeading") as IShape;

            if (shape is not null)
            {
                List<string> listItems = hotelFromDb.Amenities.Select(a => a.Name).ToList();

                shape.TextBody.Text = "";

                foreach(var item in listItems)
                {
                    //shape.TextBody.Text += item + "\n";
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);
                    paragraph.ListFormat.Type = ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';
                    textPart.Font.FontName = "system-ui";
                    textPart.Font.FontSize = 18;
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);
                }
            }
            //

            shape = slide.Shapes.FirstOrDefault(s => s.ShapeName == "imgHotel") as IShape;
            if (shape is not null)
            {
                string imageURL;
                byte[] imageData;

                try
                {
                    imageURL = string.Format("{0}{1}", basePath, hotelFromDb.ImageUrl);
                    imageData = System.IO.File.ReadAllBytes(imageURL);
                }
                catch
                {
                    imageURL = string.Format("{0}{1}", basePath, "/images/placeholder.png");
                    imageData = System.IO.File.ReadAllBytes(imageURL);
                }

                slide.Shapes.Remove(shape);
                using MemoryStream imageStream = new(imageData);
                IPicture picture = slide.Pictures.AddPicture(imageStream, 60, 120, 300, 200);
            }


            MemoryStream stream = new();
            presentation.Save(stream);
            stream.Position = 0;

            return File(stream, "application/ppt", "BookingDetails.ppt");
        }
    }
}
