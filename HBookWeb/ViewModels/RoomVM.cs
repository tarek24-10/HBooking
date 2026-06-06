using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HBookWeb.ViewModels
{
    public class RoomVM
    {
        public int RoomNumber { get; set; }
        public int HotelId { get; set; }
        [Display (Name = "Hotel List")]
        public IEnumerable<SelectListItem>? HotelList { get; set;}
        public string? SpecialDetails { get; set; }
    }
}
