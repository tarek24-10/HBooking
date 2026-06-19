using HBook.Domain.Entities;

namespace HBook.Application.Common.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        public void Update(Room entity);
    }
}
