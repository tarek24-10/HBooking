using HBook.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

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
                    Name = "SeaView Resort",
                    Description = "Comfortable seaside rooms with modern amenities.",
                    Price = 199.99,
                    Sqft = 420,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3",
                    CreatedDate = new DateTime(2024, 06, 01),
                    ModifiedDate = new DateTime(2024, 06, 01)
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Mountain Lodge",
                    Description = "Cozy lodge located in the heart of the mountains.",
                    Price = 149.50,
                    Sqft = 360,
                    Occupancy = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb",
                    CreatedDate = new DateTime(2024, 05, 15),
                    ModifiedDate = new DateTime(2024, 05, 15)
                },
                new Hotel
                {
                    Id = 3,
                    Name = "City Central Hotel",
                    Description = "Modern hotel in the city center close to attractions.",
                    Price = 179.00,
                    Sqft = 400,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1505691723518-36a3ad12d0a6",
                    CreatedDate = new DateTime(2024, 04, 20),
                    ModifiedDate = new DateTime(2024, 04, 20)
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    RoomNumber = 101,
                    HotelId = 1,
                    SpecialDetails = "Sea view, king bed"
                },
                new Room
                {
                    Id = 2,
                    RoomNumber = 102,
                    HotelId = 1,
                    SpecialDetails = "Balcony, double bed"
                },
                new Room
                {
                    Id = 3,
                    RoomNumber = 201,
                    HotelId = 2,
                    SpecialDetails = "Mountain view, fireplace"
                }
            );

            // Seed Amenity data
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "Free Wi-Fi", Description = "High-speed wireless internet access", HotelId = 1 },
                new Amenity { Id = 2, Name = "Breakfast", Description = "Complimentary continental breakfast", HotelId = 2 },
                new Amenity { Id = 3, Name = "Parking", Description = "Free on-site parking", HotelId = 3 }
            );

        }
    }
}
