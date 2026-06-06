using HBook.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HBookWeb.ViewModels
{
    public class RoomVM
    {
        public Room Room { get; set; }

        [Display (Name = "Hotel List")]
        [ValidateNever]
        public IEnumerable<SelectListItem>? HotelList { get; set;}
    }
}
