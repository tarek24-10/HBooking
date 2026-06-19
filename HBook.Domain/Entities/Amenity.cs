using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBook.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel Hotel { get; set; }
    }
}
