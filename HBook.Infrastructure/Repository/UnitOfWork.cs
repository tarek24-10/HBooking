using HBook.Application.Common.Interfaces;
using HBook.Infrastructure.Data;

namespace HBook.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IHotelRepository Hotel { get; private set; }

        public IRoomRepository Room { get; private set; }
        public IAmenityRepository Amenity { get; private set; }
        public IBookingRepository Booking { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            Hotel = new HotelRepository(context);
            Room = new RoomRepository(context);
            Amenity = new AmenityRepository(context);
            Booking = new BookingRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
