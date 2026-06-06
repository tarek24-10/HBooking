using HBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace HBook.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Room>().HasKey(r => new { r.HotelId, r.RoomNumber });

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomNumber = 101,
                    HotelId = 1,
                    SpecialDetails = "Sea view, king bed"
                },
                new Room
                {
                    RoomNumber = 102,
                    HotelId = 1,
                    SpecialDetails = "Balcony, double bed"
                },
                new Room
                {
                    RoomNumber = 201,
                    HotelId = 2,
                    SpecialDetails = "Mountain view, fireplace"
                }
            );

        }
    }
}
