using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        private readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        private readonly DateTime CurrentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusCancelled || b.Status != SD.StatusPending);

            var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= CurrentMonthStartDate
            && b.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate
            && b.BookingDate < CurrentMonthStartDate);

            return Json(GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));
        }


        public async Task<IActionResult> GetTotalRegistrationRadialChartData()
        {
            var totalUsers = _unitOfWork.ApplicationUser.GetAll();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= CurrentMonthStartDate
            && u.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt>= previousMonthStartDate
            && u.CreatedAt < CurrentMonthStartDate);

            return Json(GetRadialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetTotalRevenueRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusCancelled || b.Status != SD.StatusPending);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(b => b.TotalCost));

            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= CurrentMonthStartDate
            && u.BookingDate <= DateTime.Now).Sum(b => b.TotalCost);

            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate>= previousMonthStartDate
            && u.BookingDate < CurrentMonthStartDate).Sum(b => b.TotalCost);

            return Json(GetRadialChartDataModel(totalRevenue,Convert.ToInt32(countByCurrentMonth),Convert.ToInt32(countByPreviousMonth)));
        }

        private static RadialBarChartVM GetRadialChartDataModel(int totalCount, int countByCurrentMonth, int countByPreviousMonth)
        {
            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            //if (countByPreviousMonth != 0)
            //{
                increaseDecreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / (countByPreviousMonth == 0 ? 1 : countByPreviousMonth) * 100);
            //}

            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = countByCurrentMonth > countByPreviousMonth;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30)
            && (b.Status != SD.StatusCancelled || b.Status != SD.StatusPending));

            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomers = customerWithOneBooking.Count();

            int bookingsByReturnedCustomers = totalBookings.Count() - bookingsByNewCustomers;

            var pieChartVM = new PieChartVM
            {
                Series = new decimal[] { bookingsByNewCustomers, bookingsByReturnedCustomers },
                labels = new string[] { "New Customer Bookings", "Returned Customer Bookings" }
            };

            return Json(pieChartVM);
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30)
            && (b.Status != SD.StatusCancelled || b.Status != SD.StatusPending))
            .GroupBy(b => b.BookingDate.Date).Select(u => new { DateTime = u.Key, NewBookingCount = u.Count() });

            var CustomerData = _unitOfWork.ApplicationUser.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30))
            .GroupBy(u => u.CreatedAt.Date).Select(u => new { DateTime = u.Key, NewCustomerCount = u.Count() });

            var leftJoin = bookingData.GroupJoin(CustomerData, b => b.DateTime, c => c.DateTime,
                (b, c) => new
                {
                    b.DateTime,
                    b.NewBookingCount,
                    NewCustomerCount = c.Select(c => c.NewCustomerCount).FirstOrDefault()
                });

            var rightJoin = CustomerData.GroupJoin(bookingData, c => c.DateTime, b => b.DateTime,
                (c, b) => new
                {
                    c.DateTime,
                    NewBookingCount = b.Select(b => b.NewBookingCount).FirstOrDefault(),
                    c.NewCustomerCount,
                });

            var fullJoin = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = fullJoin.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = fullJoin.Select(x => x.NewCustomerCount).ToArray();
            var catageroies = fullJoin.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartData = new()
            {
                new ChartData()
                {
                    Name = "New Bookings",
                    Data = newBookingData
                },
                new ChartData()
                {
                     Name = "New Customers",
                    Data = newCustomerData
                }
            };

            var lineChartVM = new LineChartVM()
            {
                Categories = catageroies,
                Series = chartData
            };

            return Json(lineChartVM);
        }

    }
}
