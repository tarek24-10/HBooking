using HBook.Domain.Entities;

namespace HBook.Application.Services.Interface
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBookings(string userId = "", string? status = "");
        Booking GetBookingById(int id);
        void CreateBooking(Booking booking);
        void UpdateBookingStatus(int bookingId, string BookingStatus, int roomNumber);
        void UpdateStripePaymentId(int bookingId, string sessionId, string stripeIntentId);
        public IEnumerable<int> GetCheckedInRoomNumbers(int hotelId);
    }
}
