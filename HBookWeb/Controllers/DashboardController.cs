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

            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            if(countByPreviousMonth != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            radialBarChartVM.TotalCount = totalBookings.Count();
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = countByCurrentMonth > countByPreviousMonth;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return Json(radialBarChartVM);
        }
    }
}
