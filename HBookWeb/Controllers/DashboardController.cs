using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Application.Services.Impelementation;
using HBook.Application.Services.Interface;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        { 
            return Json(await _dashboardService.GetTotalBookingRadialChartData());
        }


        public async Task<IActionResult> GetTotalRegistrationRadialChartData()
        {

            return Json(await _dashboardService.GetTotalRegistrationRadialChartData());
        }

        public async Task<IActionResult> GetTotalRevenueRadialChartData()
        {

            return Json(await _dashboardService.GetTotalRevenueRadialChartData());
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {

            return Json(await _dashboardService.GetBookingPieChartData());
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {

            return Json(await _dashboardService.GetMemberAndBookingLineChartData());
        }

    }
}
