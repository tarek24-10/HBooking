using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Interface
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalBookingRadialChartData();
        Task<RadialBarChartDto> GetTotalRegistrationRadialChartData();
        Task<RadialBarChartDto> GetTotalRevenueRadialChartData();
        Task<PieChartDto> GetBookingPieChartData();
        Task<LineChartDto> GetMemberAndBookingLineChartData();
    }
}
