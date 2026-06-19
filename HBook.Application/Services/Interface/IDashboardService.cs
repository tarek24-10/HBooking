using HBookWeb.ViewModels;

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
