using Bogus;
using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HBook.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DbInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();

                    _userManager.CreateAsync(new ApplicationUser()
                    {
                        UserName = "tarekkhaled5112@gmail.com",
                        Email = "tarekkhaled5112@gmail.com",
                        Name = "Tarek Khaled",
                        NormalizedUserName = "TAREK KHALED",
                        NormalizedEmail = "TAREKKHALED5112@GMAIL.COM",
                        PhoneNumber = "01012345678"
                    }
                        , "Admin123*").GetAwaiter().GetResult();

                    ApplicationUser user = _context.applicationUsers.FirstOrDefault(u => u.Email == "tarekkhaled5112@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Seed()
        {
            if (_context.Hotels.Any())
                return;

            var hotels = new List<Hotel>
            {
                new()
                {
                    Name = "Seaview Resort & Spa",
                    Description = "A beachfront resort with ocean-view suites, a spa, and relaxed dining.",
                    Price = 259.00,
                    Sqft = 460,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1501117716987-c8e79b4e59f3?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new()
                {
                    Name = "Grand Meridian Hotel",
                    Description = "A modern city-center hotel with business amenities and an elegant lobby.",
                    Price = 189.00,
                    Sqft = 410,
                    Occupancy = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new()
                {
                    Name = "Alpine Crest Lodge",
                    Description = "A quiet mountain retreat with warm interiors and scenic views.",
                    Price = 169.00,
                    Sqft = 380,
                    Occupancy = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1500048993953-d23a436266cf?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new()
                {
                    Name = "Palm Harbor Suites",
                    Description = "A tropical stay with spacious suites, poolside lounging, and beach access.",
                    Price = 219.00,
                    Sqft = 435,
                    Occupancy = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                }
            };

            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();


            //-----------------------------------
            // Rooms
            //-----------------------------------
            var roomFaker = new Faker<Room>()
                .RuleFor(r => r.SpecialDetails,
                    f => f.PickRandom(
                        "Ocean view, king bed, balcony",
                        "City view, work desk, king bed",
                        "Mountain view, fireplace, king bed",
                        "Pool view, private balcony, two queen beds",
                        "Family suite, sitting area, mini fridge"));

            var rooms = new List<Room>();

            foreach (var hotel in hotels)
            {
                int roomCount = Random.Shared.Next(2, 5);

                for (int i = 1; i <= roomCount; i++)
                {
                    rooms.Add(new Room
                    {
                        HotelId = hotel.Id,
                        RoomNumber = hotel.Id * 100 + i,
                        SpecialDetails = roomFaker.Generate().SpecialDetails
                    });
                }
            }

            _context.Rooms.AddRange(rooms);
            _context.SaveChanges();


            //-----------------------------------
            // Amenities
            //-----------------------------------
            string[] amenityNames =
            {
            "Free WiFi",
            "Swimming Pool",
            "Gym",
            "Parking",
            "Spa"
        };

            var amenities = new List<Amenity>();

            foreach (var hotel in hotels)
            {
                var selectedAmenities = amenityNames
                    .OrderBy(x => Guid.NewGuid())
                    .Take(Random.Shared.Next(2, 4));

                foreach (var name in selectedAmenities)
                {
                    amenities.Add(new Amenity
                    {
                        HotelId = hotel.Id,
                        Name = name,
                        Description = $"{name} available for guests."
                    });
                }
            }

            _context.Amenities.AddRange(amenities);

            _context.SaveChanges();
        }
    }
}
