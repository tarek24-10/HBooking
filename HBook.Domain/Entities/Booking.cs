using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HBook.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required]
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public DateOnly CheckInDate { get; set; }
        public int Nights { get; set; }
        public DateOnly CheckOutDate { get; set; }
        [Required]
        public double TotalCost { get; set; }
        public string? Status { get; set; }
        public bool IsPaymentSuccessful { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        public DateTime ActualCheckInDate { get; set; }
        public DateTime ActualCheckOutDate { get; set; }

        public int RoomNumber { get; set; }
        [NotMapped]
        public List<Room> AvailableRooms { get; set; }
    }
}
