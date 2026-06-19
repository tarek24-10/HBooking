using HBook.Domain.Entities;
using HBookWeb.ViewModels;

namespace HBook.Application.Common.Utility
{
    public class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int HotelRoomsAvailable_Count(int hotelId, List<Room> roomList, List<Booking> bookingList,
            DateOnly checkInDate, int nights)
        {
            var FinalAvailableRooms = int.MaxValue;
            List<int> BookingsINDate = new List<int>();

            var RoomsInHotel = roomList.Where(r => r.HotelId == hotelId).Count();

            for (int i = 0; i < nights; i++)
            {
                var roomsBooked = bookingList.Where(b => b.HotelId == hotelId
                && b.CheckInDate <= checkInDate.AddDays(i)
                && b.CheckOutDate > checkInDate.AddDays(i));
                foreach (var booking in roomsBooked)
                {
                    if (!BookingsINDate.Contains(booking.Id))
                    {
                        BookingsINDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = RoomsInHotel - BookingsINDate.Count;
                if (totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (totalAvailableRooms < FinalAvailableRooms)
                    {
                        FinalAvailableRooms = totalAvailableRooms;
                    }
                }

            }
            return FinalAvailableRooms;
        }

        public static RadialBarChartDto GetRadialChartDataModel(int totalCount, int countByCurrentMonth, int countByPreviousMonth)
        {
            RadialBarChartDto radialBarChartDto = new();

            int increaseDecreaseRatio = 100;

            //if (countByPreviousMonth != 0)
            //{
            increaseDecreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / (countByPreviousMonth == 0 ? 1 : countByPreviousMonth) * 100);
            //}

            radialBarChartDto.TotalCount = totalCount;
            radialBarChartDto.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartDto.HasRatioIncreased = countByCurrentMonth > countByPreviousMonth;
            radialBarChartDto.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartDto;
        }
    }
}
