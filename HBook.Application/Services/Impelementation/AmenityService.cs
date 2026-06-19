using HBook.Application.Common.Interfaces;
using HBook.Application.Services.Interface;
using HBook.Domain.Entities;

namespace HBook.Application.Services.Impelementation
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckAmenityExists(int id)
        {
            return _unitOfWork.Amenity.Any(a => a.Id == id);
        }

        public void CreateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Add(amenity);
            _unitOfWork.Save();
        }

        public bool DeleteAmenity(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.Amenity.Get(a => a.Id == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Amenity.Delete(objFromDb);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Amenity> GetAllAmenities()
        {
            return _unitOfWork.Amenity.GetAll(includeproperties: "Hotel");
        }

        public Amenity GetAmenityById(int id)
        {
            return _unitOfWork.Amenity.Get(a => a.Id == id, includeproperties: "Hotel");
        }

        public void UpdateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Update(amenity);
            _unitOfWork.Save();
        }
    }
}
