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

            //-----------------------------------
            // Hotels
            //-----------------------------------
            var hotelFaker = new Faker<Hotel>()
                .RuleFor(h => h.Name, f => $"{f.Company.CompanyName()} Hotel")
                .RuleFor(h => h.Description, f => f.Lorem.Sentence())
                .RuleFor(h => h.Price, f => f.Random.Double(100, 500))
                .RuleFor(h => h.Sqft, f => f.Random.Int(500, 1500))
                .RuleFor(h => h.Occupancy, f => f.Random.Int(1, 5))
                .RuleFor(h => h.ImageUrl,
                    f => $"https://picsum.photos/seed/{f.Random.Int(1, 1000)}/800/500")
                .RuleFor(h => h.CreatedDate, f => DateTime.Now)
                .RuleFor(h => h.ModifiedDate, f => DateTime.Now);

            var hotels = hotelFaker.Generate(3);

            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();


            //-----------------------------------
            // Rooms
            //-----------------------------------
            var roomFaker = new Faker<Room>()
                .RuleFor(r => r.SpecialDetails,
                    f => f.PickRandom(
                        "Sea View",
                        "King Bed",
                        "Balcony",
                        "Mini Bar",
                        "Free Breakfast"));

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
