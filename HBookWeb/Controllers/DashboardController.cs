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

            if (countByPreviousMonth != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = countByCurrentMonth > countByPreviousMonth;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }
    }  
}
