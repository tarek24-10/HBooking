using HBook.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HBook.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Seaview Resort & Spa",
                    Description = "A beachfront resort with ocean-view suites, a full-service spa, and relaxed dining.",
                    Price = 259.00,
                    Sqft = 460,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = new DateTime(2024, 06, 01),
                    ModifiedDate = new DateTime(2024, 06, 01)
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Grand Meridian Hotel",
                    Description = "A modern city-center hotel with business amenities, an elegant lobby, and easy access to attractions.",
                    Price = 189.00,
                    Sqft = 410,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = new DateTime(2024, 05, 15),
                    ModifiedDate = new DateTime(2024, 05, 15)
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Alpine Crest Lodge",
                    Description = "A quiet mountain retreat with warm interiors, scenic views, and cozy guest rooms.",
                    Price = 169.00,
                    Sqft = 380,
                    Occupancy = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1500048993953-d23a436266cf?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = new DateTime(2024, 04, 20),
                    ModifiedDate = new DateTime(2024, 04, 20)
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Palm Harbor Suites",
                    Description = "A tropical stay with spacious suites, poolside lounging, and easy beach access.",
                    Price = 219.00,
                    Sqft = 435,
                    Occupancy = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = new DateTime(2024, 03, 28),
                    ModifiedDate = new DateTime(2024, 03, 28)
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    RoomNumber = 101,
                    HotelId = 1,
                    SpecialDetails = "Ocean view, king bed, balcony"
                },
                new Room
                {
                    Id = 2,
                    RoomNumber = 102,
                    HotelId = 1,
                    SpecialDetails = "Partial sea view, two queen beds"
                },
                new Room
                {
                    Id = 3,
                    RoomNumber = 201,
                    HotelId = 2,
                    SpecialDetails = "City view, king bed, work desk"
                },
                new Room
                {
                    Id = 4,
                    RoomNumber = 202,
                    HotelId = 2,
                    SpecialDetails = "City view, two double beds"
                },
                new Room
                {
                    Id = 5,
                    RoomNumber = 301,
                    HotelId = 3,
                    SpecialDetails = "Mountain view, fireplace, king bed"
                },
                new Room
                {
                    Id = 6,
                    RoomNumber = 302,
                    HotelId = 3,
                    SpecialDetails = "Forest view, queen bed, reading nook"
                },
                new Room
                {
                    Id = 7,
                    RoomNumber = 401,
                    HotelId = 4,
                    SpecialDetails = "Pool view, king bed, private balcony"
                },
                new Room
                {
                    Id = 8,
                    RoomNumber = 402,
                    HotelId = 4,
                    SpecialDetails = "Garden view, two queen beds"
                }
            );

            // Seed Amenity data
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "Oceanfront Pool", Description = "Heated outdoor pool overlooking the beach.", HotelId = 1 },
                new Amenity { Id = 2, Name = "Full-Service Spa", Description = "Massage, sauna, and wellness treatments.", HotelId = 1 },
                new Amenity { Id = 3, Name = "Complimentary Wi-Fi", Description = "Fast wireless internet throughout the property.", HotelId = 1 },
                new Amenity { Id = 4, Name = "Business Center", Description = "24-hour workspace with printing and meeting support.", HotelId = 2 },
                new Amenity { Id = 5, Name = "Valet Parking", Description = "Convenient valet service for hotel guests.", HotelId = 2 },
                new Amenity { Id = 6, Name = "Rooftop Lounge", Description = "City views with drinks and light bites.", HotelId = 2 },
                new Amenity { Id = 7, Name = "Fireplace Lounge", Description = "Cozy shared lounge for relaxing after a day outdoors.", HotelId = 3 },
                new Amenity { Id = 8, Name = "Ski Storage", Description = "Secure equipment storage for winter guests.", HotelId = 3 },
                new Amenity { Id = 9, Name = "Mountain Breakfast", Description = "Hot breakfast served daily with local favorites.", HotelId = 3 },
                new Amenity { Id = 10, Name = "Beach Access", Description = "Direct access to the private beach area.", HotelId = 4 },
                new Amenity { Id = 11, Name = "Airport Shuttle", Description = "Scheduled shuttle service for arrivals and departures.", HotelId = 4 },
                new Amenity { Id = 12, Name = "Family Pool", Description = "Separate pool area for family-friendly relaxation.", HotelId = 4 }
            );

        }
    }
}
